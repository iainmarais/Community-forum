import type { UserBasicInfo } from "../UserInfo";

export type ChatMessageEntry = {
    chatMessageId: string;
    chatId: string;
    chatGroupId: string;
    createdByUserId: string;
    recipientUserId: string;
    chatMessageContent: string
    createdDate: Date;
    isEdited: boolean;
    editedTime: Date;
}

export type ChatMessageBasicInfo = {
    chatMessage: ChatMessageEntry
}

export type ChatMessageFullInfo = {
    chatMessage: ChatMessageEntry,
    sender: UserBasicInfo,
    recipient: UserBasicInfo
}
