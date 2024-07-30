
export type UserInfo = {
    userId: string,
    userProfileImageBase64: string,
    username: string,
    emailAddress: string,
    userFirstname?: string,
    userLastname?: string,
    roleId: string,
    totalPosts?: number
}
export type UserBasicInfo = {
    userId: string,
    userProfileImageBase64: string,
    username: string,
    emailAddress: string,
    userFirstname?: string,
    userLastname?: string,
    roleId: string,
    totalPosts?: number
}

export type UpdateUserProfileRequest = {
    userId: string,
    userProfileImageBase64: string,
    userFirstname: string,
    userLastname: string
}

export type UserSummary = {
    totalUsers: number
}

//Enhancement: Add more request types as needed, e.g. if a user needs to reset a password they've forgotten