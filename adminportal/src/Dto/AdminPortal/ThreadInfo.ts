import type { PostBasicInfo } from "./PostInfo"
import type { TopicBasicInfo } from "./TopicInfo"
import type { UserBasicInfo } from "./UserInfo"

export type ThreadEntry = {
    threadId: string,
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
    createdByUser: UserBasicInfo,
    topic?: TopicBasicInfo
}

export type ThreadSummary = {
    totalThreads: number
}