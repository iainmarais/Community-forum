import type { LoggedInUserInfo } from "../LoggedInUserInfo"

export type AdminPortalAppState = {
    loggedInUser: LoggedInUserInfo | null,
    forumAppStats: ForumAppStats
}


export type ForumAppStats = {
    totalPosts: number,
    totalUsers: number,
    totalThreads: number,
    totalTopics: number,
    //Most active user, popular topics etc are not important for the admin portal. What I am after is totals and additionally counts per cycle, e.g. weekly, monthly etc.
}