import type { UserBasicInfo } from "../UserInfo"
import type { PostBasicInfo } from "./PostInfo"

export type Thread = {
    threadId: string,
    threadName: string,
    createdDate: Date,
    topicId: string,
    createdByUserId: string,
    numberOfPosts: number,
    hasNewPosts: boolean
}

export type ThreadBasicInfo = {
    thread: Thread
    totalPosts: number,
    newestMessage: PostBasicInfo,
    createdByUser: UserBasicInfo
}

export type ThreadFullInfo = {
    thread: Thread,
    totalPosts: number,
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

export type ThreadSummary = {
    totalThreads: number
}