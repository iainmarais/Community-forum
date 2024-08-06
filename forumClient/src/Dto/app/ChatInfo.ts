import type { UserBasicInfo } from "../UserInfo";
import type { ChatMessageBasicInfo, ChatMessageFullInfo } from "./ChatMessageInfo";

export type ChatEntry = {
    chatId: string;
    chatGroupId: string;
    chatName: string;
    createdByUserId: string;
    createdDate: string;
}

export type ChatBasicInfo = {
    chat: ChatEntry
    messages: ChatMessageBasicInfo[]
}

export type ChatFullInfo = {
    chat: ChatEntry,
    messages: ChatMessageFullInfo[],
    users: UserBasicInfo[]
}