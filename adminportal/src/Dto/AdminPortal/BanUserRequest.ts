import type { BanType } from "./UserInfo"

export type BanUserRequest = {
    banReason: string,
    banExpirationDate: Date,
    banType: string
}