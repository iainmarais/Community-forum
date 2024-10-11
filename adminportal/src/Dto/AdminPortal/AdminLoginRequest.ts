import type { ValidTokenResponse } from "@/Dto/ValidTokenResponse";
import type { UserBasicInfo } from "./UserInfo";

export type AdminLoginRequest = {
    userIdentifier: string;
    password: string;
    userContext: string;
};

export type AdminUserLoginResponse = ValidTokenResponse & {
    adminUserProfile: UserBasicInfo,
    adminUserRefreshToken:  string,
}