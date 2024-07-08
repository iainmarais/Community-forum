import type { UserBasicInfo } from "../UserInfo"

export type Message = {
    messageId: string,
    threadId: string,
    isFirstPost: boolean,
    createdByUserId: string,
    messageContent: string,
    createdDate: Date
}

export type MessageBasicInfo = {
    messageId: string,
    threadId: string,
    isFirstPost: boolean,
    createdByUserId: string,
    messageContent: string,
    createdDate: Date,
}

export type MessageFullInfo = {
    message: MessageBasicInfo,
    createdByUser: UserBasicInfo
}