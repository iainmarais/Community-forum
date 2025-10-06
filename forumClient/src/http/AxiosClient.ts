import axios, { type AxiosRequestConfig, type RawAxiosRequestHeaders } from "axios";
import { Last_Route, Token_Key, User_Refresh_Token } from "@/LocalStorage/keys";
import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import router, { HomeRoute, LoginRoute, NotFoundRoute } from '@/router';
import ErrorHandler from "@/Handlers/ErrorHandler";
import { useToast } from "vue-toastification";
import type { UserRefreshResponse } from "@/Dto/UserLoginRequest";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import { useAppContextStore } from "@/stores/AppContextStore";

const instance = axios.create({
    timeout: 10000
});
const toast = useToast();

instance.interceptors.response.use((response) => {
    return response;
}, async (error) => {
    const originalRequest = error.config as AxiosRequestConfig & { _retry?: boolean };
    
    // Check if this is a login or registration endpoint - don't auto-redirect on these
    const isAuthEndpoint = originalRequest?.url?.includes('/users/login') || 
                          originalRequest?.url?.includes('/users/register');
    
    // Handle 401 Unauthorized.
    if (error?.response?.status === 401) {
        if (error?.response?.data?.statusMessage?.includes("expired")) {
            if (!originalRequest._retry) {
                originalRequest._retry = true;
                try {
                    const currentUserId = useAppContextStore().loggedInUser?.userId;
                    const newToken = await refreshToken(currentUserId!);
                    if (newToken) {
                        SetToken(newToken);
                        console.log(`New access token is: ${newToken}`);
                        originalRequest.headers = {
                            ...originalRequest.headers,
                            Authorization: `Bearer ${newToken}`
                        };
                        return instance(originalRequest);
                    }
                } catch (refreshError) {
                    console.error("Token refresh failed:", refreshError);
                    ForceLogoff();
                    router.replace({ name: LoginRoute });
                }
            } else {
                // Token refresh already attempted, force logoff
                ForceLogoff();
                router.replace({ name: LoginRoute });
            }
        } else {
            // 401 but not expired token - likely invalid credentials
            if (!isAuthEndpoint) {
                ForceLogoff();
                router.replace({ name: LoginRoute });
            }
            // For auth endpoints, let the error propagate to the caller
        }
    } 
    // Handle 403 Forbidden
    else if (error?.response?.status === 403) {
        ErrorHandler.handleApiErrorResponse(error);
        if (!isAuthEndpoint) {
            toast.error("You don't have permission to access this resource.");
        }
    } 
    // Handle 404 Not Found
    else if (error?.response?.status === 404) {
        console.error("Not found error:", error);
        ErrorHandler.handleApiErrorResponse(error);
        if (!isAuthEndpoint) {
            router.push({ name: NotFoundRoute });
        }
    } 
    // Handle network errors (no response from server - CORS, connection refused, etc.)
    else if (!error.response) {
        console.error("Network error:", {
            message: error.message,
            code: error.code,
            url: originalRequest?.url
        });
        
        if (!isAuthEndpoint) {
            toast.error("Could not establish a connection to the server. Please try again later.");
            router.replace({ name: LoginRoute });
        }
        // For auth endpoints, let the error propagate so the login page can handle it
    } 
    // Handle other server errors (5xx)
    else {
        ErrorHandler.handleApiErrorResponse(error);
        if (!isAuthEndpoint) {
            toast.error("A server error occurred. Please try again later.");
        }
    }
    
    return Promise.reject(error);
});

const refreshToken = async (userId: string) => {
    const refreshToken = localStorage.getItem(User_Refresh_Token);
    if(refreshToken) {
        const refreshTokenRequest = {
            refreshToken: refreshToken,
            loggedInUserId: userId
        };
        try {
            const response = await Post<UserRefreshResponse>(
                `${ConfigurationLoader.getConfig().apiV1.baseUrl}/users/auth/refresh`, 
                refreshTokenRequest
            );
            // Check that the response payload is not null
            if (response.data) {
                return response.data.newAccessToken;
            }
        } catch (error) {
            console.error("Failed to refresh token:", error);
            return null;
        }
    }
    return null;
}

const ForceLogoff = () => {
    RemoveToken();
    localStorage.removeItem(Token_Key);
    localStorage.removeItem(User_Refresh_Token);
}

export const SetToken = (token: string) => {
    instance.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

const RemoveToken = () => {
    instance.defaults.headers.common['Authorization'] = '';
}

const Get = async <T>(url: string): Promise<ApiSuccessResponse<T>> => {
    const response = await instance.get(url);
    return response.data as ApiSuccessResponse<T>;
}

const Put = async <T>(url: string, data: any): Promise<ApiSuccessResponse<T>> => {
    const response = await instance.put(url, data);
    return response.data as ApiSuccessResponse<T>;
}

const Post = async <T>(url: string, data: any, headers?: any): Promise<ApiSuccessResponse<T>> => {
    const response = await instance.post(url, data, { headers });
    return response.data as ApiSuccessResponse<T>;
}

const Delete = async <T>(url: string, data: any): Promise<ApiSuccessResponse<T>> => {
    const response = await instance.delete(url, data);
    return response.data as ApiSuccessResponse<T>;
}

// Blob responses
const Post_WithBlobResponse = async <T>(url: string, data: any): Promise<any> => {
    const response = await instance.post(url, data, { responseType: 'blob' });
    return response.data as ApiSuccessResponse<T>;
}

const Get_WithBlobResponse = async <T>(url: string): Promise<any> => {
    const response = await instance.get(url, { responseType: 'blob' });
    return response.data as ApiSuccessResponse<T>;
}

const headers: RawAxiosRequestHeaders = {
    'Content-Type': 'application/json'
};

export default {
    Get,
    Put,
    Post,
    Delete,
    Post_WithBlobResponse,
    Get_WithBlobResponse,
    ForceLogoff,
    refreshToken
};