/*
    Administration state store

    This store is under construction. Use cases are for anything to do with administration of the forum such as adding and editing users' details, assigning roles, deleting posts, images etc.
*/
import type { RoleBasicInfo } from "@/Dto/RoleInfo";
import type { CreateUserRequest, UserBasicInfo } from "@/Dto/UserInfo";
import { defineStore } from "pinia";

import { useToast } from "vue-toastification";

import AdminService from "@/services/Admin/AdminService";
import ErrorHandler from "@/Handlers/ErrorHandler";

const toast = useToast();

type AdminStoreState = {
    selectedUser?: UserBasicInfo;
    selectedUserId?: string;
    selectedRoleId?: string;

    users: UserBasicInfo[] | undefined;
    roles: RoleBasicInfo[] | undefined;

    loading_createUser: boolean;
    result_createUserSuccess: boolean;

    loading_updateUser: boolean;
    result_updateUserSuccess: boolean;

    loading_gettingUsers: boolean;
    result_getUsersSuccess: boolean;

    loading_gettingRoles: boolean;
    result_getRolesSuccess: boolean;

    loading_assignUserRole: boolean;
    result_assignUserRoleSuccess: boolean;

};

const defaultState: AdminStoreState = {
    users: [],
    roles: [],

    loading_createUser: false,
    result_createUserSuccess: false,

    loading_updateUser: false,
    result_updateUserSuccess: false,

    loading_gettingUsers: false,
    result_getUsersSuccess: false,

    loading_gettingRoles: false,
    result_getRolesSuccess: false,

    loading_assignUserRole: false,
    result_assignUserRoleSuccess: false
};

export const useAdminStore = defineStore({
    id: "AdminStore",
    state: () => (defaultState),
    getters: {
        
    },
    actions: {
        getRoles() {
            this.loading_gettingRoles = true;
            AdminService.getRoles().then(response =>{
                this.loading_gettingRoles = false;
                this.result_getRolesSuccess = true;
                this.roles = response.data;
            }, error => {
                this.loading_gettingRoles = false;
                this.result_getRolesSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        },
        createUser(req: CreateUserRequest) {
            this.loading_createUser = true;
            AdminService.createUser(req).then(response => {
                this.loading_createUser = false;
                this.result_createUserSuccess = true;
                toast.Success("User created successfully");
            }, error => {
                this.loading_createUser = false;
                this.result_createUserSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        },
        updateUser (userToUpdate: UserBasicInfo) {
            this.loading_updateUser = true;
            AdminService.updateUser(userToUpdate).then(response => {
                this.loading_updateUser = false;
                this.result_updateUserSuccess = true;
                toast.Success("User updated successfully");
            }, error => {
                this.loading_updateUser = false;
                this.result_updateUserSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        },
        getUsers() {
            this.loading_gettingUsers = true;
            AdminService.getUsers().then(response => {
                this.loading_gettingUsers = false;
                this.result_getUsersSuccess = true;
                this.users = response.data;
            }, error => {
                this.loading_gettingUsers = false;
                this.result_getUsersSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        },
        assignUserRole() {

        }
    }
});