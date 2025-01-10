import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { AddUserRequest } from "@/Dto/AdminPortal/AddUserRequest";
import type { AssignRoleRequest } from "@/Dto/AdminPortal/AssignRoleRequest";
import type { BanUserRequest } from "@/Dto/AdminPortal/BanUserRequest";
import type { RoleEntry } from "@/Dto/AdminPortal/RoleInfo";
import type { UpdateUserRequest } from "@/Dto/AdminPortal/UpdateUserRequest";
import type { BannedUserBasicInfo, BannedUserSummary, UserBasicInfo, UserEntry, UserFullInfo, UserSummary } from "@/Dto/AdminPortal/UserInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import AdminPortalService from "@/Services/AdminPortalService";
import { defineStore } from "pinia";
import { useToast } from "vue-toastification";

const toast = useToast();

type UserManagementStore = {
    //Pulling this in as paginated data.
    users: PaginatedData<UserFullInfo[], UserSummary>;

    bannedUsers: PaginatedData<BannedUserBasicInfo[], BannedUserSummary>;

    loading_getUserInfo: boolean;
    result_getUserInfoSuccess: boolean;

    loading_getBannedUserInfo: boolean;
    result_getBannedUserInfoSuccess: boolean;
    
    loading_banUserInProgress: boolean;
    result_banUserSuccess: boolean;

    loading_addUserInProgress: boolean;
    result_addUserSuccess: boolean;

    loading_gettingUserRoles: boolean;
    result_getUserRolesSuccess: boolean;

    loading_deleteUserInProgress: boolean;
    result_deleteUserSuccess: boolean;

    loading_assignUserRoleInProgress: boolean;
    result_assignUserRoleSuccess: boolean;

    loading_updateUserInProgress: boolean;
    result_updateUserSuccess: boolean;

    selectedUser?: UserBasicInfo;
    bannedUser? : BannedUserBasicInfo;

    searchQuery?: string;

    currentPageNumber: number;
    rowsPerPage: number;

    banUserRequest?: BanUserRequest;

    addUserRequest?: AddUserRequest;

    userRoles: RoleEntry[];
}

const defaultState: UserManagementStore = {
    users: {
        rows: [],
        pageNumber: 0,
        totalPages: 0,
        totalRows: 0,
        summary: {
            totalUsers: 0,
            mostActiveUsers: 0,
            newUsers: 0
        }
    },

    bannedUsers: {
        rows: [],
        pageNumber: 0,
        totalPages: 0,
        totalRows: 0,
        summary: {
            totalBannedUsers: 0,
            permanentlyBannedUsers: 0
        } 
    },

    loading_getUserInfo: false,
    result_getUserInfoSuccess: false,

    loading_getBannedUserInfo: false,
    result_getBannedUserInfoSuccess: false,

    loading_banUserInProgress: false,
    result_banUserSuccess: false,

    loading_addUserInProgress: false,
    result_addUserSuccess: false,

    loading_gettingUserRoles: false,
    result_getUserRolesSuccess: false,

    loading_deleteUserInProgress: false,
    result_deleteUserSuccess: false,

    loading_assignUserRoleInProgress: false,
    result_assignUserRoleSuccess: false,

    loading_updateUserInProgress: false,
    result_updateUserSuccess: false,

    selectedUser: undefined,

    currentPageNumber: 1,
    rowsPerPage: 10,

    userRoles: [] as RoleEntry[]
}

export const useUserManagementStore = defineStore({
    id: "UserManagementStore",
    state: () => (defaultState),
    getters: {},
    actions: {
        getUserInfo() {
            this.loading_getUserInfo = true;
            AdminPortalService.getUserInfo(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading_getUserInfo = false;
                this.result_getUserInfoSuccess = true;
                this.users = response.data;
            }, error => {
                this.loading_getUserInfo = false;
                this.result_getUserInfoSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        getBannedUserInfo() {
            this.loading_getBannedUserInfo = true;
            AdminPortalService.getBannedUserInfo(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading_getBannedUserInfo = false;
                this.result_getBannedUserInfoSuccess = true;
                this.bannedUsers = response.data;
            }, error => {
                this.loading_getBannedUserInfo = false;
                this.result_getBannedUserInfoSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        banUser (userId: string, request: BanUserRequest) {
            this.loading_banUserInProgress = true;
            AdminPortalService.banUser(userId, request).then(response => {
                this.loading_banUserInProgress = false;
                this.result_banUserSuccess = true;
                this.bannedUser = response.data;
            }, error => {
                this.loading_banUserInProgress = false;
                this.result_banUserSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        addUser (request: AddUserRequest) {
            this.loading_addUserInProgress = true;
            AdminPortalService.addUser(request).then(response => {
                this.loading_addUserInProgress = false;
                this.result_addUserSuccess = true;
                this.users = response.data;
            }, error => {
                this.loading_addUserInProgress = false;
                this.result_addUserSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        deleteUser (userId: string) {
            this.loading_deleteUserInProgress = true;
            AdminPortalService.deleteUser(userId).then(response => {
                this.loading_deleteUserInProgress = false;
                this.result_deleteUserSuccess = true;
                toast.success("User deleted successfully");
            }, error => {
                this.loading_deleteUserInProgress = false;
                this.result_deleteUserSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        getUserRoles() {
            this.loading_gettingUserRoles = true;
            AdminPortalService.getUserRoles().then(response => {
                this.loading_gettingUserRoles = false;
                this.result_getUserRolesSuccess = true;
                this.userRoles = response.data;
            }, error => {
                this.loading_gettingUserRoles = false;
                this.result_getUserRolesSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        assignUserRole(userId: string, request: AssignRoleRequest) {
            this.loading_assignUserRoleInProgress = true;
            AdminPortalService.assignUserRole(userId, request).then(response => {
                this.loading_assignUserRoleInProgress = false;
                this.result_assignUserRoleSuccess = true;
                toast.success("User role updated successfully");
            }, error => {
                this.loading_assignUserRoleInProgress = false;
                this.result_assignUserRoleSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        updateUser(userId: string, request: UpdateUserRequest) {
            this.loading_updateUserInProgress = true;
            AdminPortalService.updateUser(userId, request).then(response =>{
                this.loading_updateUserInProgress = false;
                this.result_updateUserSuccess = true;
                toast.success("User updated successfully.");
            }, error => {
                this.loading_updateUserInProgress = false;
                this.result_updateUserSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        }
    }
})