import type { TopicBasicInfo } from "./TopicInfo"
import type { UserBasicInfo } from "./UserInfo"

export type BoardEntry = {
    boardId: string
    categoryId: string,
    createdByUserId: string,
    boardName: string,
    boardDescription: string,
}

export type BoardBasicInfo = {
    board: BoardEntry,
    createdByUser: UserBasicInfo,
    numTopics: number
}

export type BoardFullInfo = {
    board: BoardEntry,
    createdByUser: UserBasicInfo,
    totalTopics: number,
    topics: TopicBasicInfo[]
}

export type BoardSummary = {
    totalBoards: number,
    boardsPerCategory: number
}