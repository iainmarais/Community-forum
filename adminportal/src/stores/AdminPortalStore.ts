import type { AdminPortalAppState, AdminPortalStats } from "@/Dto/AdminPortal/AdminPortalAppState";
import type { LoggedInUserInfo } from "@/Dto/LoggedInUserInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import AdminPortalService from "@/Services/AdminPortalService";
import { defineStore } from "pinia";

type AdminPortalStore = {
    loading_getAdminPortalAppState: boolean;
    result_getAdminPortalAppStateSuccess: boolean;

    currentLoggedInUser: LoggedInUserInfo;
    appStats: AdminPortalStats;
}

const defaultState: AdminPortalStore = {
    loading_getAdminPortalAppState: false,
    result_getAdminPortalAppStateSuccess: false,

    currentLoggedInUser: {
        userId: "",
        userFirstname: "",
        userLastname: "",
        roleName: "",
        userProfileImageBase64: ""
    } as LoggedInUserInfo,
    appStats: {
        totalCategories: 0,
        totalBoards: 0,
        totalUsers: 0,
        totalThreads: 0,
        totalTopics: 0,
        totalPosts: 0,
        totalGalleryItems: 0,
        totalImages: 0,
        totalVideos: 0,
        totalFiles:  0,
        totalAudioFiles: 0,
    } as AdminPortalStats,
}

export const useAdminPortalStore = defineStore({
    id: "AdminPortalStore",
    state: () => (defaultState),
    getters: {},
    actions: {
        getAdminPortalAppState() {
            this.loading_getAdminPortalAppState = true;
            AdminPortalService.getAdminPortalAppState().then(response => {
                this.loading_getAdminPortalAppState = false;
                this.result_getAdminPortalAppStateSuccess = true;
                this.currentLoggedInUser =  {...this.currentLoggedInUser, ...response.data.currentLoggedInUser};
                this.appStats = {...this.appStats, ...response.data.adminPortalStats};
            }, error => {
                this.loading_getAdminPortalAppState = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        }
    }
})