import axios, {type RawAxiosRequestHeaders} from "axios";
import { LastRoute, TokenKey } from "@/LocalStorage/keys";
import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import router, { HomeRoute, LoginRoute, NotFoundRoute } from '@/router';
import ErrorHandler from "@/Handlers/ErrorHandler";

const instance = axios.create();

instance.interceptors.response.use((response) => {
    return response;
}, (error) => {
    if (error?.response?.status === 401 && error.response.config.url) {
            // Chucked a sickie 'cause you're not allowed, mate.
            ForceLogoff();
            router.replace({ name: LoginRoute });
        } else if (error?.response?.status === 403) {
            // Sorry mate, you don't have the authorisation to go there.
            ErrorHandler.handleApiErrorResponse(error);
        } else if (error?.response?.status === 404) {
            // Well... bollocks. We can't find that sodding page! 
            router.push ({ name: NotFoundRoute });
            console.error("Not found error:", error);
            ErrorHandler.handleApiErrorResponse(error);
        router.replace({name:HomeRoute});
        } else {
            // Looks like the server's hit a snag. Fair dinkum.
            ErrorHandler.handleApiErrorResponse(error);
        }
    return Promise.reject(error);
});


const ForceLogoff = () => {
    RemoveToken();
    localStorage.removeItem(TokenKey);
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

const Post = async <T>(url: string, data: any, headers?: RawAxiosRequestHeaders) : Promise<ApiSuccessResponse<T>> => {
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

export default {
    Get,
    Put,
    Post,
    Delete,
    Post_WithBlobResponse,
    Get_WithBlobResponse,
    ForceLogoff
};