import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { UserBasicInfo, UserEntry, UserSummary } from "@/Dto/AdminPortal/UserInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import AdminPortalService from "@/Services/AdminPortalService";
import { defineStore } from "pinia";

type UserManagementStore = {
    //Pulling this in as paginated data.
    users: PaginatedData<UserBasicInfo[], UserSummary>;
    loading_getUserInfo: boolean;
    result_getUserInfoSuccess: boolean;

    selectedUser?: UserBasicInfo;
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
    loading_getUserInfo: false,
    result_getUserInfoSuccess: false,

    selectedUser: undefined,
}

export const useUserManagementStore = defineStore({
    id: "UserManagementStore",
    state: () => (defaultState),
    getters: {},
    actions: {
        getUserInfo() {
            this.loading_getUserInfo = true;
            AdminPortalService.getUserInfo().then(response => {
                this.loading_getUserInfo = false;
                this.result_getUserInfoSuccess = true;
                this.users = response.data;
            }, error => {
                this.loading_getUserInfo = false;
                this.result_getUserInfoSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        }
    }
})