import type { UpdateUserProfileRequest, UserBasicInfo } from "@/Dto/UserInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import UserService from "@/services/UserService";
import { defineStore } from "pinia";

type UserProfileStoreState  = {
    user: UserBasicInfo,

    loading_updateUserProfile: boolean,
    result_updateUserProfileSuccess: boolean
}

const defaultState: UserProfileStoreState = {
    user: {} as UserBasicInfo,

    loading_updateUserProfile: false,
    result_updateUserProfileSuccess: false
}

export const useUserProfileStore = defineStore({
    id: "UserProfileStore",
    state: () => (defaultState),
    getters: {
    },
    actions: {
        updateUserProfile(request: UpdateUserProfileRequest) {
            this.loading_updateUserProfile = true;
            this.result_updateUserProfileSuccess = false;
            UserService.updateUserProfile(request).then((response) => {
                this.user = response.data;
                this.result_updateUserProfileSuccess = true;
                this.loading_updateUserProfile = false;
            }, error => {
                this.result_updateUserProfileSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        }
    }
});