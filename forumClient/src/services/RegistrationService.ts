import ConfigurationLoader from '@/config/ConfigurationLoader';
import AxiosClient from '@/http/AxiosClient';
import type { ApiSuccessResponse } from '@/ApiResponses/ApiSuccessResponse';
import type { UserBasicInfo, UserRegistrationRequest } from '@/Dto/UserInfo';

const RegisterUser = async (request: UserRegistrationRequest): Promise<ApiSuccessResponse<UserBasicInfo>> => {
    return AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/users/register`, request);
}

export default {
    RegisterUser
}