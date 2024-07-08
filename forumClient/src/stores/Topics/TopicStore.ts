import { defineStore } from "pinia";
import type { TopicFullInfo } from "@/Dto/app/TopicInfo";
import TopicService from "@/services/TopicService";
import ErrorHandler from "@/Handlers/ErrorHandler";

export type TopicStoreState = {
    topic: TopicFullInfo;
    loading_getTopicFullInfo: boolean;
    result_getTopicFullInfoSuccess: boolean;
}

const defaultState: TopicStoreState = {
    topic: {} as TopicFullInfo,
    loading_getTopicFullInfo: false,
    result_getTopicFullInfoSuccess: false
}

export const useTopicStore = defineStore({
    id: "TopicStore",
    state: () => defaultState,
    getters: {
    },
    actions: {
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
            });
        }
    }
});
