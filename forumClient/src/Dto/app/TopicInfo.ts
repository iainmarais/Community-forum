import type { UserBasicInfo } from "../UserInfo"
import type { ThreadBasicInfo } from "./ThreadInfo"

export type TopicEntry = {
    topicId: string,
    categoryId: string,
    topicName: string,
    description: string,
    createdByUserId: string,
    createdDate: Date
}

export type TopicBasicInfo = {
    topicId: string,
    categoryId: string,
    topicName: string,
    description: string,
    createdByUserId: string,
    createdDate: Date,
    numNewThreads: number,
    numTotalThreads: number
    threads: ThreadBasicInfo[],
    totalPosts: number
}

export type TopicFullInfo = {
    topic: TopicBasicInfo,
    threads: ThreadBasicInfo[],
    totalThreads: number,
    createdByUser: UserBasicInfo
}

export type CreateTopicRequest = {
    categoryId: string,
    topicName: string,
    description: string,
}