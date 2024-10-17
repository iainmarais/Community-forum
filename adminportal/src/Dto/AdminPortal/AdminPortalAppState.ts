import type { LoggedInUserInfo } from "../LoggedInUserInfo"

export type AdminPortalAppState = {
    currentLoggedInUser: LoggedInUserInfo | null,
    adminPortalStats: AdminPortalStats
}

export type AdminPortalStats = {
    totalCategories: number;
    totalBoards: number;
    totalUsers: number;
    totalThreads: number;
    totalTopics: number;
    totalPosts: number;
    totalGalleryItems: number;
    totalImages: number;
    totalVideos: number;
    totalFiles: number;
    totalAudioFiles: number;
}