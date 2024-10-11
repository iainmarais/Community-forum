import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { AdminLoginRequest, AdminUserLoginResponse } from "@/Dto/AdminPortal/AdminLoginRequest";
import type { AdminPortalAppState } from "@/Dto/AdminPortal/AdminPortalAppState";
import type { LoggedInUserInfo } from "@/Dto/LoggedInUserInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const adminLogin = async (request: AdminLoginRequest) : Promise<ApiSuccessResponse<AdminUserLoginResponse>> => {
    //Login to the admin portal
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/admin/login`, request);
}

const getAdminPortalAppState = async (): Promise<ApiSuccessResponse<AdminPortalAppState>> => {
    //Get the admin portal app state from the server
    return await AxiosClient.Get<AdminPortalAppState>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/appstate`);
}


export default {
    getAdminPortalAppState,
    adminLogin
}