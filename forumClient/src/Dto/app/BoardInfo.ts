import type { TopicBasicInfo } from "./TopicInfo"
export type BoardEntry = {
    boardId: string,
    categoryId: string,
    boardName: string,
    boardDescription: string,
    createdByUserId: string
}

export type BoardBasicInfo = {
    boardId: string,
    boardName: string,
    boardDescription: string,
    createdByUserId: string, 
    topics: TopicBasicInfo[], 
}

export type BoardFullInfo = {
    board: BoardBasicInfo,
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