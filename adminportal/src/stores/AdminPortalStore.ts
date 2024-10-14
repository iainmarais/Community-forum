import type { AdminPortalAppState } from "@/Dto/AdminPortal/AdminPortalAppState";
import ErrorHandler from "@/Handlers/ErrorHandler";
import AdminPortalService from "@/Services/AdminPortalService";
import { defineStore } from "pinia";

type AdminPortalStore = {
    loading_getAdminPortalAppState: boolean;
    result_getAdminPortalAppStateSuccess: boolean;

    adminPortalAppState: AdminPortalAppState;
}

const defaultState: AdminPortalStore = {
    loading_getAdminPortalAppState: false,
    result_getAdminPortalAppStateSuccess: false,

    adminPortalAppState: {} as AdminPortalAppState
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
                this.adminPortalAppState = response.data;
            }, error => {
                this.loading_getAdminPortalAppState = false;
                this.result_getAdminPortalAppStateSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        }
    }
})