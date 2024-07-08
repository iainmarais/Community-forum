import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { UserInfo } from "@/Dto/UserInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getUserById = async (userId: string): Promise<ApiSuccessResponse<UserInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/users/${userId}`);
}

export default {
    getUserById
}