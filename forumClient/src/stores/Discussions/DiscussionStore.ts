import type { ThreadFullInfo } from "@/Dto/app/ThreadInfo";
import { defineStore } from "pinia";
import DiscussionService from "@/services/DiscussionService";
import ErrorHandler from "@/Handlers/ErrorHandler";
import type { CreatePostRequest, PostBasicInfo, PostFullInfo, PostSummary } from "@/Dto/app/PostInfo";
import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";

type DiscussionStoreState = {
    thread: ThreadFullInfo | undefined,
    post: PostFullInfo | undefined,
    posts?: PaginatedData<PostFullInfo[], PostSummary>, 

    loading_getThreadFullInfo: boolean,
    result_discussionSuccess: boolean,

    loading_createPost: boolean,
    result_createPostSuccess: boolean,

    loading_getPosts: boolean,
    result_getPostsSuccess: boolean,

    currentPageNumber: number,
    rowsPerPage: number,

    searchQuery?: string
}

const defaultState: DiscussionStoreState = {
    thread: {} as ThreadFullInfo,
    post: undefined,
    posts: {} as PaginatedData<PostFullInfo[], PostSummary>,

    loading_getThreadFullInfo: false,
    result_discussionSuccess: false,

    loading_createPost: false,
    result_createPostSuccess: false,

    loading_getPosts: false,
    result_getPostsSuccess: false,

    currentPageNumber: 1,
    rowsPerPage: 10,
}

export const useDiscussionStore = defineStore({
    id: "DiscussionStore",
    state: () => (defaultState),
    getters: {
    },
    actions: {
        getThreadFullInfo(threadId: string) {
            this.loading_getThreadFullInfo = true;
            this.result_discussionSuccess = false;
            return DiscussionService.getThreadFullInfo(threadId).then((response) => {
                this.thread = response.data;
                this.result_discussionSuccess = true;
                this.loading_getThreadFullInfo = false;
            }, error => {
                this.result_discussionSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        },
        getPostsForThread() {
            this.loading_getPosts = true;
            DiscussionService.getPostsForThread(this.thread!.thread.threadId, this.currentPageNumber, this.rowsPerPage, this.searchQuery).then((response) => {
                this.posts = response.data;
                this.result_getPostsSuccess = true;
                this.loading_getPosts = false;
            }, error => {
                this.result_getPostsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        
        },

        createPost(request: CreatePostRequest) {
            this.loading_createPost = true;
            this.result_createPostSuccess = false;
            DiscussionService.postReply(request).then((response) => {
                this.posts = response.data;
                this.result_createPostSuccess = true;
                this.loading_createPost = false;
            }, error => {
                this.result_createPostSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        }
    }
});