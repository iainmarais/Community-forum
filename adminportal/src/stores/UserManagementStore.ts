import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
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

    selectedUser?: UserBasicInfo;

    searchQuery?: string;

    currentPageNumber: number;
    rowsPerPage: number;
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

    selectedUser: undefined,

    currentPageNumber: 1,
    rowsPerPage: 10,
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
        }
    }
})