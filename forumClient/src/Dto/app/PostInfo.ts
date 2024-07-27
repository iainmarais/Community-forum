import type { UserBasicInfo } from "../UserInfo"

export type Post = {
    postId: string,
    threadId: string,
    isFirstPost: boolean,
    createdByUserId: string,
    postContent: string,
    createdDate: Date,
    replyToPostId: string,
    postReported: boolean,
    reportReason: string,
    reportedByUserId: string
}

export type PostBasicInfo = {
    postId: string,
    threadId: string,
    isFirstPost: boolean,
    createdByUserId: string,
    postContent: string,
    createdDate: Date,
    replyToPostId: string,
    postReported: boolean,
    reportReason: string,
    reportedByUserId: string
}

export type PostFullInfo = {
    message: PostBasicInfo,
    createdByUser: UserBasicInfo
}

export type CreatePostRequest = {
    threadId: string,
    isFirstPost: boolean,
    createdByUserId: string,
    postContent: string,
    replyToPostId: string
}

export type ReportPostRequest = {
    threadId: string,
    postId: string,
    reportReason: string,
    reportedByUserId: string,
}