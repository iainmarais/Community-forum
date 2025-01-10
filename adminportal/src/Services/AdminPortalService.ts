import { ApiSuccessResponse, type PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { AddUserRequest } from "@/Dto/AdminPortal/AddUserRequest";
import type { AdminLoginRequest, AdminUserLoginResponse } from "@/Dto/AdminPortal/AdminLoginRequest";
import type { AdminPortalAppState } from "@/Dto/AdminPortal/AdminPortalAppState";
import type { AssignRoleRequest } from "@/Dto/AdminPortal/AssignRoleRequest";
import type { BanUserRequest } from "@/Dto/AdminPortal/BanUserRequest";
import type { RoleEntry } from "@/Dto/AdminPortal/RoleInfo";
import type { UpdateUserRequest } from "@/Dto/AdminPortal/UpdateUserRequest";
import type { BannedUserBasicInfo, BannedUserSummary, UserBasicInfo, UserFullInfo, UserSummary } from "@/Dto/AdminPortal/UserInfo";
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

const getUserInfo = async (pageNumber: number, rowsPerPage: number, searchTerm?: string): Promise<ApiSuccessResponse<PaginatedData<UserFullInfo[],UserSummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if(searchTerm) {
        queryParams.push(`searchTerm=${searchTerm}`);
    }    
    var res = await AxiosClient.Get<PaginatedData<UserFullInfo[], UserSummary>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/users?${queryParams.join('&')}`);
    return res;
}

const getBannedUserInfo = async (pageNumber: number, rowsPerPage: number, searchTerm?: string): Promise<ApiSuccessResponse<PaginatedData<BannedUserBasicInfo[],BannedUserSummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if(searchTerm) {
        queryParams.push(`searchTerm=${searchTerm}`);
    }    
    var res = await AxiosClient.Get<PaginatedData<BannedUserBasicInfo[], BannedUserSummary>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/users/banned?${queryParams.join('&')}`);
    return res;
}

const banUser = async (userId: string, request: BanUserRequest) : Promise<ApiSuccessResponse<BannedUserBasicInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/users/${userId}/ban`, request);
}

const addUser = async (req:AddUserRequest): Promise<ApiSuccessResponse<UserBasicInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/users/add`, req);
}

const getUserRoles = async (): Promise<ApiSuccessResponse<RoleEntry[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/users/roles`)
}
//User id is being sent via the url, so no need for any additional data to be sent. The server only expects the user id.
const deleteUser = async (userId: string): Promise<ApiSuccessResponse<object>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/users/${userId}/delete`, null)
}

const assignUserRole = async (userId: string, request: AssignRoleRequest): Promise<ApiSuccessResponse<object>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/users/${userId}/assignrole`, request)
}

const updateUser = async (userId: string, request: UpdateUserRequest): Promise<ApiSuccessResponse<UserBasicInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/users/${userId}/update`, request)
}
export default {
    getAdminPortalAppState,
    adminLogin,
    getUserInfo,
    getBannedUserInfo, 
    banUser,
    addUser,
    getUserRoles,
    deleteUser,
    assignUserRole,
    updateUser
}