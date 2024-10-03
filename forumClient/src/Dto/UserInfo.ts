import type { ValidTokenResponse } from "./ValidTokenResponse"

export type UserEntry = {
    userId: string,
    userProfileImageBase64: string,
    username: string,
    emailAddress: string,
    address?: string,
    cityName?: string,
    countryName?: string,
    postalCode?: string,
    userFirstname?: string,
    userLastname?: string,
    gender?:  string,
    roleId: string,
    totalPosts?: number,
    isOnline: boolean,
    isVisible: boolean,
    lastLoginTime: Date
}
export type UserBasicInfo = {
    user: UserEntry,
    userFullName: string
}

export type UpdateUserProfileRequest = {
    userId: string,
    userProfileImageBase64: string,
    userFirstname: string,
    userLastname: string,
    address: string,
    cityName: string,
    countryName: string,
    postalCode: string,
    gender: string
}

//Can only refresh token when user is logged in. This should be done during auth with the existing token, but if not this can serve as a fallback.
//During authentication with stored credentials from the previous session, if the token expires, the user will be kicked off. I want to allow them to stay logged in.
export type RefreshUserTokenRequest = {
    currentUserAccessToken: string,
    currentUserId: string,
    currentUsername: string
}

export type UserSummary = {
    totalUsers: number
}

export type UserRegistrationRequest = {
    username: string, 
    emailAddress: string, 
    roleType?: string,
    password: string,
    retypePassword: string
}

export type CreateUserRequest = {
   username: string,
   emailAddress: string,
   roleName: string,
   password: string,
   retypePassword: string, 
}

//Enhancement: Add more request types as needed, e.g. if a user needs to reset a password they've forgotten