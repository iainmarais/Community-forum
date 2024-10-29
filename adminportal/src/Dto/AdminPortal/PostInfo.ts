import type { ThreadBasicInfo } from "./ThreadInfo"
import type { TopicBasicInfo } from "./TopicInfo"
import type { UserBasicInfo } from "./UserInfo"

export type PostEntry = {
    postId: string,
    threadId: string,
    isFirstPost: boolean,
    createdByUserId: string,
    postContent: string,
    createdDate: Date,
    replyToPostId: string,
    postReported: boolean,
    reportReason: string,
    reportedByUserId: string,
}

export type PostBasicInfo = {
    post: PostEntry,
    createdByUser: UserBasicInfo,
    thread?: ThreadBasicInfo
}

export type PostSummary = {
    totalPosts: number
}