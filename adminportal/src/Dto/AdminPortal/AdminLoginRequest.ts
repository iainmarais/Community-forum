import type { ValidTokenResponse } from "@/Dto/ValidTokenResponse";
import type { UserBasicInfo } from "./UserInfo";

export type AdminLoginRequest = {
    userIdentifier: string;
    password: string;
    userContext: string;
};

export type AdminUserLoginResponse = ValidTokenResponse & {
    refreshToken:  string,
    adminUserProfile: UserBasicInfo,
}

export type AdminUserRefreshResponse = ValidTokenResponse & {
    refreshToken: string,
    //Not sure if I need to return the user profile along with the incoming dataset, but for continuity with the login response, will include it.
    adminUserProfile: UserBasicInfo
}