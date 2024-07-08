import type { TopicBasicInfo } from "./TopicInfo"

export type CategoryEntry = {
    categoryId: string,
    categoryName: string,
    description: string
}

export type CategoryBasicInfo = {
    categoryId: string,
    categoryName: string,
    description: string,
    topics: TopicBasicInfo[],
}

export type CategoryFullInfo = {
    category: CategoryBasicInfo,
    topics: TopicBasicInfo[],
    totalTopics: number
}