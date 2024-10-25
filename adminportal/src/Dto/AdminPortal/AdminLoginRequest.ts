import type { ValidTokenResponse } from "@/Dto/ValidTokenResponse";
import type { UserBasicInfo } from "./UserInfo";

export type AdminLoginRequest = {
    userIdentifier: string;
    password: string;
    userContext: string;
};

export type AdminUserLoginResponse = ValidTokenResponse & {
    adminUserRefreshToken:  string,
    adminUserProfile: UserBasicInfo,
}

export type AdminUserRefreshResponse = {
    newAccessToken: string,
    newAccessTokenExpiration: string,
    refreshToken: string,
    //Not sure if I need to return the user profile along with the incoming dataset, but for continuity with the login response, will include it.
    adminUserProfile: UserBasicInfo
}