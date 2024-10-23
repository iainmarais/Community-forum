import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { AddUserRequest } from "@/Dto/AdminPortal/AddUserRequest";
import type { BanUserRequest } from "@/Dto/AdminPortal/BanUserRequest";
import type { RoleEntry } from "@/Dto/AdminPortal/RoleInfo";
import type { BannedUserBasicInfo, BannedUserSummary, UserBasicInfo, UserEntry, UserFullInfo, UserSummary } from "@/Dto/AdminPortal/UserInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import AdminPortalService from "@/Services/AdminPortalService";
import { defineStore } from "pinia";

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

    selectedUser?: UserBasicInfo;

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
            AdminPortalService.banUser(userId, request).then(response => {
                this.bannedUsers = response.data;
            }, error => {
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        addUser (request: AddUserRequest) {
            AdminPortalService.addUser(request).then(response => {
                this.users = response.data;
            }, error => {
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
    }
})