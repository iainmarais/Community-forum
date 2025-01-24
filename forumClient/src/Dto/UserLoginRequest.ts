import type { UserBasicInfo } from "./UserInfo"
import type { ValidTokenResponse } from "./ValidTokenResponse"

export type UserLoginRequest = {
    userIdentifier: string,
    password: string,
    userContext: string
}

export type UserLoginResponse = ValidTokenResponse & {
    refreshToken: string,
    forumUserProfile: UserBasicInfo
}

export type UserRefreshResponse = {
    newAccessToken: string,
    newAccessTokenExpiration: string,
    refreshToken: string,
    forumUserProfile: UserBasicInfo
}