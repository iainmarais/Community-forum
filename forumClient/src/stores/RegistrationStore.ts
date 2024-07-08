import type { UserInfo } from "@/Dto/UserInfo";
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
    userResult: UserInfo
}

const defaultState: RegistrationStoreState = {
    username: "",
    emailAddress: "",
    password: "",
    retypePassword: "",

    loading_RegistrationInProgress: false,
    result_RegistrationSuccess: false,
    userResult: {} as UserInfo
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

        }
    }
});