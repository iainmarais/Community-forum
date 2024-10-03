import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";
import type { ApiSuccessResponse, PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { CreateUserRequest, UserBasicInfo } from "@/Dto/UserInfo";
import type { RoleBasicInfo } from "@/Dto/RoleInfo";

const getUsers = async (): Promise<ApiSuccessResponse<UserBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/admin/users`);
}

const createUser = async (req: CreateUserRequest): Promise<ApiSuccessResponse<void>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/admin/users/create`, req);
}

const updateUser = async (user: UserBasicInfo): Promise<ApiSuccessResponse<void>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/admin/users/update`, user);
}


const getRoles = async (): Promise<ApiSuccessResponse<RoleBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/admin/roles`);
}


export default {
    getUsers,
    getRoles,
    createUser,
    updateUser
}