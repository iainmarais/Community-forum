import type { CreateTopicRequest, TopicBasicInfo, TopicFullInfo } from "@/Dto/app/TopicInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import ForumService from "@/services/ForumService";
import TopicService from "@/services/TopicService";
import { defineStore } from "pinia";

type TopicListStoreState = {
    topics: TopicBasicInfo[],
    newestTopics: TopicBasicInfo[],
    hasNewPosts: boolean
    loading_getTopics: boolean,
    result_getTopicsSuccess: boolean,

    loading_getNewestTopics: boolean,
    result_getNewestTopicsSuccess: boolean,

    loading_getTopicFullInfo: boolean,
    result_getTopicFullInfoSuccess: boolean,

    loading_createNewTopic: boolean,
    result_createNewTopicSuccess: boolean,

    topicFullInfo?: TopicFullInfo
}

const defaultState: TopicListStoreState = {
    topics: [] as TopicBasicInfo[],
    newestTopics: [] as TopicBasicInfo[],
    hasNewPosts: false,
    loading_getTopics: false,
    result_getTopicsSuccess: false,

    loading_getNewestTopics: false,
    result_getNewestTopicsSuccess: false,

    loading_getTopicFullInfo: false,
    result_getTopicFullInfoSuccess: false,

    loading_createNewTopic: false,
    result_createNewTopicSuccess: false
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
        },
        getTopicFullInfo(topicId: string) {
            this.loading_getTopicFullInfo = true;
            this.result_getTopicFullInfoSuccess = false;
            TopicService.getTopicFullInfo(topicId).then((response) => {
                this.topicFullInfo = response.data;
                this.result_getTopicFullInfoSuccess = true;
                this.loading_getTopicFullInfo = false;
            }, error => {
                this.result_getTopicFullInfoSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
                this.loading_getTopicFullInfo = false;
            })
        },
        createNewTopic(request:CreateTopicRequest) {
            this.loading_createNewTopic = true;
            this.result_createNewTopicSuccess = false;
            TopicService.createNewTopic(request).then((response) => {
                this.topicFullInfo = response.data;
                this.result_createNewTopicSuccess = true;
                this.loading_createNewTopic = false;
            }, error => {
                this.result_createNewTopicSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
                this.loading_createNewTopic = false;
            })
        }
    }
})