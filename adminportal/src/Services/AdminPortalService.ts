import type { ApiSuccessResponse, PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { AdminLoginRequest, AdminUserLoginResponse } from "@/Dto/AdminPortal/AdminLoginRequest";
import type { AdminPortalAppState } from "@/Dto/AdminPortal/AdminPortalAppState";
import type { UserBasicInfo, UserSummary } from "@/Dto/AdminPortal/UserInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const adminLogin = async (request: AdminLoginRequest) : Promise<ApiSuccessResponse<AdminUserLoginResponse>> => {
    //Login to the admin portal
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/login`, request);
}

const getAdminPortalAppState = async (): Promise<ApiSuccessResponse<AdminPortalAppState>> => {
    //Get the admin portal app state from the server
    var res = await AxiosClient.Get<AdminPortalAppState>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/appstate`);
    console.log(res);
    return res;
}

const getUserInfo = async (): Promise<ApiSuccessResponse<PaginatedData<UserBasicInfo[],UserSummary>>> => {
    var res = await AxiosClient.Get<PaginatedData<UserBasicInfo[], UserSummary>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/users`);
    return res;
}


export default {
    getAdminPortalAppState,
    adminLogin,
    getUserInfo
}