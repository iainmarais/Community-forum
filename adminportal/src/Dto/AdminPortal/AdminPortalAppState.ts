import type { LoggedInUserInfo } from "../LoggedInUserInfo"

export type AdminPortalAppState = {
    loggedInUser: LoggedInUserInfo | null,
    adminPortalStats: AdminPortalStats
}

export type AdminPortalStats = {
    TotalCategories: number;
    TotalUsers: number;
    TotalThreads: number;
    TotalTopics: number;
    TotalPosts: number;
    TotalGalleryItems: number;
    TotalImages: number;
    TotalVideos: number;
    TotalFiles: number;
    TotalAudioFiles: number;
}