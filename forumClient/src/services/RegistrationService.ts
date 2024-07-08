import ConfigurationLoader from '@/config/ConfigurationLoader';
import AxiosClient from '@/http/AxiosClient';
import type { ApiSuccessResponse } from '@/ApiResponses/ApiSuccessResponse';
import type { UserInfo } from '@/Dto/UserInfo';

const RegisterUser = async (request: {username: string, emailAddress: string, password: string, retypePassword: string}): Promise<ApiSuccessResponse<UserInfo>> => {
    return AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/users/register`, request);
}

export default {
    RegisterUser
}