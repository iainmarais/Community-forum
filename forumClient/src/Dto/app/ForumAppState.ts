import type { UserBasicInfo } from "@/Dto/UserInfo";

export type LoggedInUserInfo = {
    userId: string,
    userFirstname?: string,
    userLastname?: string,
    roleName?: string
    userProfileImageBase64?: string
    //Add additional props here.
}

export type ForumAppState = {
    forumStats: ForumStats,
    loggedInUser: LoggedInUserInfo | null
}

export type ForumStats = {
    totalPosts: number,
    totalUsers: number,
    totalThreads: number,
    totalTopics: number,
    //Users with the highest post counts. These should be done by the backend and not kept in the db but identified by how many posts have the same user id, +-10 is a good count.
    mostActiveUsers?: UserBasicInfo[],
    popularTopics: number
    //Add additional props here.
}