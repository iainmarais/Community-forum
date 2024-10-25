import type { UserBasicInfo } from "./UserInfo"

export type BoardEntry = {
    categoryId: string,
    createdByUserId: string,
    boardName: string,
    boardDescription: string,
}

export type BoardBasicInfo = {
    board: BoardEntry,
    createdByUser: UserBasicInfo
}

export type BoardSummary = {
    totalBoards: number,
    boardsPerCategory: number
}