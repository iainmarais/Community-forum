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
    totalPosts: number
}

export type TopicFullInfo = {
    topic: TopicBasicInfo,
    threads: ThreadBasicInfo[],
    totalThreads: number,
    createdByUser: UserBasicInfo
}