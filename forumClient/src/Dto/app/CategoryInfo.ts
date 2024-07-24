import type { TopicBasicInfo } from "./TopicInfo"

export type CategoryEntry = {
    categoryId: string,
    categoryName: string,
    categoryDescription: string
}

export type CategoryBasicInfo = {
    categoryId: string,
    categoryName: string,
    categoryDescription: string,
    topics: TopicBasicInfo[],
}

export type CategoryFullInfo = {
    category: CategoryBasicInfo,
    topics: TopicBasicInfo[],
    totalTopics: number
}

export type CreateCategoryRequest = {
    categoryName: string,
    categoryDescription: string
}