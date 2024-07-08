import type { ForumStats } from "@/Dto/app/ForumAppState";
import ErrorHandler from "@/Handlers/ErrorHandler";
import ForumService from "@/services/ForumService";
import { defineStore } from "pinia";

type ForumStatsStoreState = {
    loading_getForumStats: boolean,
    result_getForumStatsSuccess: boolean,
    forumStats: ForumStats
}

const defaultState: ForumStatsStoreState = {
    loading_getForumStats: false,
    result_getForumStatsSuccess: false,
    forumStats: {
        totalPosts: 0,
        totalUsers: 0,
        totalThreads: 0,
        totalTopics: 0,
        mostActiveUsers: [],
        popularTopics: 0
    }
}


export const useForumStatsStore = defineStore({
    id: "ForumStatsStore",
    state: () => (defaultState),
    getters: {},
    actions: {
        getForumStats() {
            this.loading_getForumStats = true;
            ForumService.getForumAppState().then(response => {
                this.forumStats = response.data.forumStats;
                this.result_getForumStatsSuccess = true;
                this.loading_getForumStats = false;
            }, error => {
                this.result_getForumStatsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
                this.loading_getForumStats = false;
            });
        }
    }
});