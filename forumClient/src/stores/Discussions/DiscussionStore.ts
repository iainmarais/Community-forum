import type { ThreadFullInfo } from "@/Dto/app/ThreadInfo";
import { defineStore } from "pinia";
import DiscussionService from "@/services/DiscussionService";
import ErrorHandler from "@/Handlers/ErrorHandler";

type DiscussionStoreState = {
    thread: ThreadFullInfo | undefined,
    loading_discussion: boolean,
    result_discussionSuccess: boolean
}

const defaultState: DiscussionStoreState = {
    thread: undefined,
    loading_discussion: false,
    result_discussionSuccess: false
}

export const useDiscussionStore = defineStore({
    id: "DiscussionStore",
    state: () => (defaultState),
    getters: {
    },
    actions: {
        getThreadFullInfo(threadId: string) {
            this.loading_discussion = true;
            this.result_discussionSuccess = false;
            DiscussionService.getThreadFullInfo(threadId).then((response) => {
                this.thread = response.data;
                this.result_discussionSuccess = true;
                this.loading_discussion = false;
            }, error => {
                this.result_discussionSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        }
    }
});