import type { BoardBasicInfo } from "./BoardInfo"

export type CategoryEntry = {
    categoryId: string,
    categoryName: string,
    categoryDescription: string,
    createdByUserId: string, 
}

export type CategoryBasicInfo = {
    category: CategoryEntry,
    boards: BoardBasicInfo[]
}

export type CategorySummary = {
    totalCategories: number
}