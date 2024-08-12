import type { ForumStats, LoggedInUserInfo } from "@/Dto/app/ForumAppState";
import ErrorHandler from "@/Handlers/ErrorHandler";
import { createSignalRConnection } from "@/http/SignalRConnection";
import ForumService from "@/services/ForumService";
import { defineStore } from "pinia";

type ForumStatsStoreState = {
    loading_getForumStats: boolean,
    result_getForumStatsSuccess: boolean,
    forumStats: ForumStats,
    loggedInUser: LoggedInUserInfo | null
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
    },
    loggedInUser: {} as LoggedInUserInfo
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
                this.loggedInUser = response.data.loggedInUser;
                this.result_getForumStatsSuccess = true;
                this.loading_getForumStats = false;
            }, error => {
                this.result_getForumStatsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
                this.loading_getForumStats = false;
            });
        },
        updateForumStats() {
            //Use signalR to update the forum stats. This can be triggered by a watcher on the vue component.
            const forumStatsConn = createSignalRConnection("forumStats", []);
            forumStatsConn.on("GetForumStats", (userId: string, updatedForumStats: ForumStats) => {
                userId = this.loggedInUser!.userId;
                this.forumStats = updatedForumStats;
                this.result_getForumStatsSuccess = true;
                console.log("Updated forum stats");
            });
            forumStatsConn.start()
                .then(() => console.log("Connection to forum stats hub started"))
                .catch((err) => ErrorHandler.handleApiErrorResponse(err));
        }
    }
});