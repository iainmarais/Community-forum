export type PostEntry = {
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
}

export type PostSummary = {
    totalPosts: number
}