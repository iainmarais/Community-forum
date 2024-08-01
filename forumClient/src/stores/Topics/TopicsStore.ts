import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { ThreadBasicInfo, ThreadSummary } from "@/Dto/app/ThreadInfo";
import type { CreateTopicRequest, TopicBasicInfo, TopicFullInfo } from "@/Dto/app/TopicInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import ForumService from "@/services/ForumService";
import TopicService from "@/services/TopicService";
import { defineStore } from "pinia";

type TopicsStoreState = {
    topic: TopicFullInfo | undefined,
    threads: PaginatedData<ThreadBasicInfo[], ThreadSummary>,
    newestTopics: TopicBasicInfo[],
    hasNewPosts: boolean,

    loading_getTopics: boolean,
    result_getTopicsSuccess: boolean,

    loading_getNewestTopics: boolean,
    result_getNewestTopicsSuccess: boolean,

    loading_getTopicFullInfo: boolean,
    result_getTopicFullInfoSuccess: boolean,

    loading_createNewTopic: boolean,
    result_createNewTopicSuccess: boolean,
    
    currentPageNumber: number,
    rowsPerPage: number,

    searchQuery?: string
}

const defaultState: TopicsStoreState = {
    topic: undefined,
    threads: {} as PaginatedData<ThreadBasicInfo[], ThreadSummary>,
    newestTopics: [] as TopicBasicInfo[],
    hasNewPosts: false,

    loading_getTopics: false,
    result_getTopicsSuccess: false,

    loading_getNewestTopics: false,
    result_getNewestTopicsSuccess: false,

    loading_getTopicFullInfo: false,
    result_getTopicFullInfoSuccess: false,

    loading_createNewTopic: false,
    result_createNewTopicSuccess: false,

    currentPageNumber: 1,
    rowsPerPage: 10,
}

export const useTopicsStore = defineStore({
    id: "TopicsStore",
    state: () => (defaultState),
    getters: {
    },
    actions: {
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
                this.topic = response.data;
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
                this.topic = response.data;
                this.result_createNewTopicSuccess = true;
                this.loading_createNewTopic = false;
            }, error => {
                this.result_createNewTopicSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
                this.loading_createNewTopic = false;
            })
        },

        getThreadsForTopic() {
            this.loading_getTopics = true;
            this.result_getTopicsSuccess = false;
            if(!this.topic){
                return;
            }
            TopicService.getThreadsForTopic(this.topic?.topic.topicId, this.currentPageNumber, this.rowsPerPage, this.searchQuery).then((response) => {
                this.threads = response.data;
                this.result_getTopicsSuccess = true;
                this.loading_getTopics = false;
            }, error => {
                this.result_getTopicsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
                this.loading_getTopics = false;
            })
        }

    }
})