import axios, { type AxiosRequestConfig, type RawAxiosRequestHeaders } from "axios";
import { Last_Route, Token_Key, User_Refresh_Token } from "@/LocalStorage/keys";
import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import router, { HomeRoute, LoginRoute, NotFoundRoute } from '@/router';
import ErrorHandler from "@/Handlers/ErrorHandler";
import { useToast } from "vue-toastification";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import type { AdminUserRefreshResponse } from "@/Dto/AdminPortal/AdminLoginRequest";

const instance = axios.create({
    timeout: 10000
});
const toast = useToast();

instance.interceptors.response.use(
    (response) => response, // Pass through successful responses
    async (error) => {
        const originalRequest = error.config as AxiosRequestConfig & { _retry?: boolean };
        //Server sends through an ApiErrorResponse for errors. 
        if (error?.response?.status === 401) {
            if (error?.response?.data?.statusMessage?.includes("expired")) {
                // Only retry the refresh once to avoid an infinite loop
                if (!originalRequest._retry) {
                    originalRequest._retry = true; // Mark request as retried

                    try {
                        // Refresh the token and retry the original request
                        const newToken = await refreshToken();
                        if (newToken) {
                            SetToken(newToken);
                            originalRequest.headers['Authorization'] = `Bearer ${newToken}`;
                            return instance(originalRequest); // Retry the original request
                        }
                    } catch (refreshError) {
                        // Handle refresh token failure (e.g., token invalid or server issue)
                        console.error("Token refresh failed:", refreshError);
                        ForceLogoff();
                        router.replace({ name: LoginRoute });
                    }
                }
            } else {
                // Log off if the error isn’t due to an expired token
                ForceLogoff();
                router.replace({ name: LoginRoute });
            }
        } else if (error?.response?.status === 403) {
            // Forbidden - handle as needed
            ErrorHandler.handleApiErrorResponse(error);
        } else if (error?.response?.status === 404) {
            // Not found - navigate to a custom 404 page or home
            router.push({ name: NotFoundRoute });
            console.error("Not found error:", error);
            ErrorHandler.handleApiErrorResponse(error);
        } else if (!error.response) {
            // Network/server error - notify the user and redirect to login
            toast.error("Could not establish a connection to the server. Please try again later.");
            router.replace({ name: LoginRoute });
        } else {
            // General server error - log and handle as needed
            ErrorHandler.handleApiErrorResponse(error);
        }
        
        return Promise.reject(error);
    }
);

const refreshToken = async () => {
    const refreshToken = localStorage.getItem(User_Refresh_Token);
    if(refreshToken) {
        const response = await Post<ApiSuccessResponse<AdminUserRefreshResponse>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/auth/refresh`, refreshToken);
        return response.data.data.accessToken;
    }
}

const ForceLogoff = () => {
    RemoveToken();
    localStorage.removeItem(Token_Key);
}

export const SetToken = (token:string) => {
    instance.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

const RemoveToken = () => {
    instance.defaults.headers.common['Authorization'] = ``;
}

const Get = async <T>(url: string) : Promise<ApiSuccessResponse<T>> => {
    const response = await instance.get(url);
    return response.data as ApiSuccessResponse<T>;
}

const Put = async <T>(url: string, data: any) : Promise<ApiSuccessResponse<T>> => {
    const response = await instance.put(url, data);
    return response.data as ApiSuccessResponse<T>;
}

const Post = async <T>(url: string, data: any, headers?: any) : Promise<ApiSuccessResponse<T>> => {
    const response = await instance.post(url, data, { headers });
    return response.data as ApiSuccessResponse<T>;
}

const Delete = async <T>(url: string, data: any) : Promise<ApiSuccessResponse<T>> => {
    const response = await instance.delete(url, data);
    return response.data as ApiSuccessResponse<T>;
}

//Blob responses

const Post_WithBlobResponse = async <T>(url: string, data: any) : Promise<any> => {
    const response = await instance.post(url, data, {responseType: 'blob'});
    return response.data as ApiSuccessResponse<T>;
}

const Get_WithBlobResponse = async <T>(url: string) : Promise<any> => {
    const response = await instance.get(url, {responseType: 'blob'});
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
    ForceLogoff
};