import type { ThreadBasicInfo } from "./ThreadInfo"
import type { UserBasicInfo, UserFullInfo } from "./UserInfo"

export type TopicEntry = {
    boardId: string,
    topicName: string,
    description: string,
    createdByUserId: string,
    createdDate: Date
}

export type TopicBasicInfo = {
    topic: TopicEntry,
    totalThreads: number,
    totalPosts: number,
    numNewthreads: number,
    newestThread: ThreadBasicInfo,
    createdByUser: UserBasicInfo
}

export type TopicSummary = {
    totalTopics: number
}