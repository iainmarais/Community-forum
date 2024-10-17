import type { UserBasicInfo, UserEntry } from "@/Dto/AdminPortal/UserInfo";
import { defineStore } from "pinia";

type UserManagementStore = {
    users: UserBasicInfo[];
    loading_getUserInfo: boolean;
    selectedUser?: UserBasicInfo;
}

const defaultState: UserManagementStore = {
    users: [],
    loading_getUserInfo: false,
    selectedUser: undefined,
}

export const useUserManagementStore = defineStore({
    id: "UserManagementStore",
    state: () => (defaultState),
    getters: {},
    actions: {}
})