
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

export type UserSummary = {
    totalUsers: number
}

//Enhancement: Add more request types as needed, e.g. if a user needs to reset a password they've forgotten