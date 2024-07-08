import type { TopicBasicInfo } from "@/Dto/app/TopicInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import ForumService from "@/services/ForumService";
import { defineStore } from "pinia";

type TopicListStoreState = {
    topics: TopicBasicInfo[],
    newestTopics: TopicBasicInfo[],
    hasNewPosts: boolean
    loading_getTopics: boolean,
    result_getTopicsSuccess: boolean,

    loading_getNewestTopics: boolean,
    result_getNewestTopicsSuccess: boolean
}

const defaultState: TopicListStoreState = {
    topics: [] as TopicBasicInfo[],
    newestTopics: [] as TopicBasicInfo[],
    hasNewPosts: false,
    loading_getTopics: false,
    result_getTopicsSuccess: false,

    loading_getNewestTopics: false,
    result_getNewestTopicsSuccess: false
}

export const useTopicListStore = defineStore({
    id: "TopicListStore",
    state: () => (defaultState),
    getters: {
    },
    actions: {
        getTopics() {
            this.loading_getTopics = true;
            this.result_getTopicsSuccess = false;
            ForumService.getTopics().then((response) => {
                this.topics = response.data;
                this.result_getTopicsSuccess = true;
                this.loading_getTopics = false;
            }, error => {
                this.result_getTopicsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            })
        },

        getNewestTopics() {
            this.loading_getNewestTopics = true;
            this.result_getNewestTopicsSuccess = false;
            ForumService.getNewestTopics().then((response) => {
                this.newestTopics = response.data;
                this.result_getNewestTopicsSuccess = true;
                this.loading_getNewestTopics = false;
            }, error => {
                this.result_getNewestTopicsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
                this.loading_getNewestTopics = false;
            })
        }
    }
})