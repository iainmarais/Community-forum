import type { ThreadFullInfo } from "@/Dto/app/ThreadInfo";
import { defineStore } from "pinia";
import DiscussionService from "@/services/DiscussionService";
import ErrorHandler from "@/Handlers/ErrorHandler";
import type { CreatePostRequest, PostFullInfo } from "@/Dto/app/PostInfo";

type DiscussionStoreState = {
    thread: ThreadFullInfo | undefined,
    post: PostFullInfo | undefined,

    loading_discussion: boolean,
    result_discussionSuccess: boolean,

    loading_createPost: boolean,
    result_createPostSuccess: boolean
}

const defaultState: DiscussionStoreState = {
    thread: undefined,
    post: undefined,

    loading_discussion: false,
    result_discussionSuccess: false,

    loading_createPost: false,
    result_createPostSuccess: false
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
        },

        createPost(request: CreatePostRequest) {
            this.loading_createPost = true;
            this.result_createPostSuccess = false;
            DiscussionService.postReply(request).then((response) => {
                this.post = response.data;
                this.result_createPostSuccess = true;
                this.loading_createPost = false;
            }, error => {
                this.result_createPostSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        }
    }
});