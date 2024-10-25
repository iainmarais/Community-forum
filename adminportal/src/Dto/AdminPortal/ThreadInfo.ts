import type { PostBasicInfo } from "./PostInfo"
import type { UserBasicInfo } from "./UserInfo"

export type ThreadEntry = {
    threadName: string,
    createdDate: Date,
    topicId: string,
    createdByUserId: string,
    numberOfPosts: number,
    hasNewPosts: boolean,
}

export type ThreadBasicInfo = {
    thread: ThreadEntry,
    totalPosts: number,
    newestMessage: PostBasicInfo,
    createdByUser: UserBasicInfo
}

export type ThreadSummary = {
    totalThreads: number
}