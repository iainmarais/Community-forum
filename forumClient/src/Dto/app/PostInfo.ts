import type { UserBasicInfo } from "../UserInfo"

export type Post = {
    postId: string,
    threadId: string,
    isFirstPost: boolean,
    createdByUserId: string,
    postContent: string,
    createdDate: Date
}

export type PostBasicInfo = {
    postId: string,
    threadId: string,
    isFirstPost: boolean,
    createdByUserId: string,
    postContent: string,
    createdDate: Date,
}

export type PostFullInfo = {
    message: PostBasicInfo,
    createdByUser: UserBasicInfo
}