import type { BoardBasicInfo } from "./BoardInfo"
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
    boards: BoardBasicInfo[],
}

export type CategoryFullInfo = {
    category: CategoryBasicInfo,
    boards: BoardBasicInfo[],
    totalBoards: number
}

export type CreateCategoryRequest = {
    categoryName: string,
    categoryDescription: string
}

export type CategorySummary = {
    totalCategories: number
}