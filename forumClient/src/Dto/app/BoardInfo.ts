import type { TopicBasicInfo } from "./TopicInfo"
export type BoardEntry = {
    boardId: string,
    categoryId: string,
    boardName: string,
    boardDescription: string,
    createdByUserId: string
}

export type BoardBasicInfo = {
    board: BoardEntry, 
}

export type BoardFullInfo = {
    board: BoardEntry,
    topics: TopicBasicInfo[],
    totalTopics: number
}

export type CreateBoardRequest = {
    boardName: string,
    boardDescription: string,
    categoryId: string,
}

export type BoardSummary = {
    totalBoards: number
}