import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { RefreshUserTokenRequest, UpdateUserProfileRequest, UserBasicInfo } from "@/Dto/UserInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getUserById = async (userId: string): Promise<ApiSuccessResponse<UserBasicInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/users/${userId}`);
}

const updateUserProfile = async (request: UpdateUserProfileRequest): Promise<ApiSuccessResponse<UserBasicInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/users/${request.userId}/profile`, request);
}
//Todo: build out.
const refreshUserToken = async (req: RefreshUserTokenRequest) : Promise<ApiSuccessResponse<UserBasicInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/users/${req.currentUserId}/refresh`, req);
}

export default {
    getUserById,
    updateUserProfile,
    refreshUserToken
}