import type { UserBasicInfo } from "../UserInfo"
import type { PostBasicInfo } from "./PostInfo"

export type Thread = {
    threadId: string,
    threadName: string,
    dateCreated: Date,
    topicId: string,
    threadStarterId: string,
    numberOfPosts: number,
    hasNewPosts: boolean
}

export type ThreadBasicInfo = {
    threadId: string,
    threadName: string,
    createdDate: Date,
    topicId: string,
    createdByUserId: string,
    numberOfPosts: number,
    hasNewPosts: boolean,
    createdByUsername?: string,
    createdByUserFirstname?: string,
    createdByUserLastname?: string
}

export type ThreadFullInfo = {
    thread: ThreadBasicInfo,
    posts: PostBasicInfo[],
    createdByUser: UserBasicInfo
}

export type CreateThreadRequest = {
    topicId: string,
    threadName: string,
    createdByUserId: string,
}

export type CreateThreadWithPostRequest = {
    topicId: string,
    threadName: string,
    createdByUserId: string,
    messageContent: string,
    
}

