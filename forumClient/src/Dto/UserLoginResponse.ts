import type { UserBasicInfo } from "./UserInfo";
import type { ValidTokenResponse } from "./ValidTokenResponse";

export type UserLoginResponse = ValidTokenResponse & {
    userProfile: UserBasicInfo
}