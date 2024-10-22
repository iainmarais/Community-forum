import type { RoleEntry } from "./RoleInfo"

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
    registrationTime: Date,
    lastLoginTime: Date
}
export type UserBasicInfo = {
    user: UserEntry,
    userFullName: string
}

export type UserFullInfo = {
    user: UserEntry,
    role: RoleEntry,
    contacts?: UserBasicInfo[],
    userFullName: string
}

export type UserSummary = {
    totalUsers: number, 
    mostActiveUsers: number,
    newUsers: number
}