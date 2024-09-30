import type { UserBasicInfo, UserEntry } from "@/Dto/UserInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import RegistrationService from "@/services/RegistrationService";
import { defineStore } from "pinia";

type RegistrationStoreState = {
    username: string,
    emailAddress: string,
    password: string,
    retypePassword: string,

    loading_RegistrationInProgress: boolean,
    result_RegistrationSuccess: boolean,
    userResult: UserBasicInfo
}

const defaultState: RegistrationStoreState = {
    username: "",
    emailAddress: "",
    password: "",
    retypePassword: "",

    loading_RegistrationInProgress: false,
    result_RegistrationSuccess: false,
    userResult: {} as UserBasicInfo
}

export const useRegistrationStore = defineStore({
    id: "RegistrationStore",
    state: () => (defaultState),
    getters: {
    },
    actions: {

        RegisterUser(request: {username: string, emailAddress: string, password: string, retypePassword: string}) {
            this.loading_RegistrationInProgress = true;
            RegistrationService.RegisterUser(request).then((response) => {
                this.loading_RegistrationInProgress = false;
                this.result_RegistrationSuccess = true;
                this.userResult = response.data;
            }, error => {
                this.loading_RegistrationInProgress = false;
                this.result_RegistrationSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            })

        },
        RegisterUserWithRole(request: {username: string, emailAddress: string, roleType: string, password: string, retypePassword: string}) {
            this.loading_RegistrationInProgress = true;
            RegistrationService.RegisterUser(request).then((response) => {
                this.loading_RegistrationInProgress = false;
                this.result_RegistrationSuccess = true;
                this.userResult = response.data;
            }, error => {
                this.loading_RegistrationInProgress = false;
                this.result_RegistrationSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        }
    }
});