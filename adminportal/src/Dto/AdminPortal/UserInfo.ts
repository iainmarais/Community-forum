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

export type BannedUserEntry = {
    userId: string,
    banType: BanType,
    banReason: string,
    banExpirationDate: Date
}

export enum BanType {
    Permanent, //Users permanently banned will not be given a reprieve unless they are reinstated. They may appeal it.
    Administrative, //As a result of actions taken by administrators or moderators. These usually last for a specified time period, typically a year.
    UserRequested //User requested their account be banned. This is useful in the case a user's email address was stolen or otherwise compromised.
}