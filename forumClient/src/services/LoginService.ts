import ConfigurationLoader from '@/config/ConfigurationLoader';
import AxiosClient from '@/http/AxiosClient';
import type { ApiSuccessResponse } from '@/ApiResponses/ApiSuccessResponse';
import type { UserLoginResponse } from '@/Dto/UserLoginResponse';
import type { UserLoginRequest } from '@/Dto/UserLoginRequest';

const LoginUser = async (request:UserLoginRequest): Promise<ApiSuccessResponse<UserLoginResponse>> => {
    try {
        const url = `${ConfigurationLoader.getConfig().apiV1.baseUrl}/users/login`;
        console.log("Attempting login to: " + url + "\nwith request: " + JSON.stringify(request));
        var response: ApiSuccessResponse<UserLoginResponse> = await AxiosClient.Post(url, request);
        console.log("Response: " + JSON.stringify(response));
        return response;
        
    } catch (error: any) {
        console.error(`Login failed.\n${error}`);
            console.error('Error details:', {
            message: error.message,
            response: error.response?.data,
            status: error.response?.status,
            headers: error.response?.headers,
            code: error.code,
            config: error.config
        });
        return Promise.reject(error);
    }
}

export default {
    LoginUser
}