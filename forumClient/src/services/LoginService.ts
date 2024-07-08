import ConfigurationLoader from '@/config/ConfigurationLoader';
import AxiosClient from '@/http/AxiosClient';
import type { ApiSuccessResponse } from '@/ApiResponses/ApiSuccessResponse';
import type { UserLoginResponse } from '@/Dto/UserLoginResponse';
import type { UserLoginRequest } from '@/Dto/UserLoginRequest';

const LoginUser = async (request:UserLoginRequest): Promise<ApiSuccessResponse<UserLoginResponse>> => {
    return AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/users/login`, request);
}

export default {
    LoginUser
}