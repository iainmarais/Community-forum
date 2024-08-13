import type { ForumAppState, ForumStats, LoggedInUserInfo } from "@/Dto/app/ForumAppState";
import ErrorHandler from "@/Handlers/ErrorHandler";
import createSignalRConnection, { type ConnectionRequest } from "@/http/SignalRConnection";
import ForumService from "@/services/ForumService";
import { defineStore } from "pinia";
import { useSignalRConnectionStore } from "./SignalRConnectionStore";

type ForumStatsStoreState = {
    loading_getForumStats: boolean,
    result_getForumStatsSuccess: boolean,
    forumStats: ForumStats,
    loggedInUser: LoggedInUserInfo | null,
}

const signalRConnectionStore = useSignalRConnectionStore();
const forumStatsConnection = signalRConnectionStore.signalRConnection;

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
    loggedInUser: {} as LoggedInUserInfo,
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
        connectToForumStatsHub() {
            signalRConnectionStore.connectToHub("forumstats", "GetForumStats", (messageType, ...args) => {this.handleSignalRMessage(messageType, ...args)} ,this.loggedInUser?.userId);
        },
        updateForumStats() {
            signalRConnectionStore.sendMessage("GetForumStats", [this.loggedInUser?.userId]);
        },
        handleSignalRMessage (messageType: string, ...args: string[]) {
            console.log(`Handling message of type ${messageType}, with args:`, args)
        }
    }
});