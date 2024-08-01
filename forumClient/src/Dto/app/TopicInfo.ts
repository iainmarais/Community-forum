import type { UserBasicInfo } from "../UserInfo"
import type { ThreadBasicInfo } from "./ThreadInfo"

export type TopicEntry = {
    topicId: string,
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
    numTotalThreads: number,
    numNewThreads: number,
    newestThread: ThreadBasicInfo,
    createdByUser: UserBasicInfo
}

export type TopicFullInfo = {
    topic: TopicEntry,
    totalPosts: number,
    numTotalThreads: number,
    numNewThreads: number,
    newestThread: ThreadBasicInfo,
    threads: ThreadBasicInfo[],
    totalThreads: number,
    createdByUser: UserBasicInfo
}

export type CreateTopicRequest = {
    boardId: string,
    topicName: string,
    description: string,
}

export type TopicSummary = {
    totalTopics: number
}