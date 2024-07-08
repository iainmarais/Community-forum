import type { CreateThreadRequest, CreateThreadWithPostRequest, ThreadBasicInfo } from "@/Dto/app/ThreadInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import ForumService from "@/services/ForumService";
import { defineStore } from "pinia";

type ThreadListStoreState = {
    threads: ThreadBasicInfo[],
    loading_getThreads: boolean,
    result_getThreadsSuccess: boolean,

    thread: ThreadBasicInfo | undefined,
    loading_createThread: boolean,
    result_createThreadSuccess: boolean
}

const defaultState: ThreadListStoreState = {
    threads: [] as ThreadBasicInfo[],
    loading_getThreads: false,
    result_getThreadsSuccess: false,

    thread: undefined,
    loading_createThread: false,
    result_createThreadSuccess: false
}

export const useThreadListStore = defineStore({
    id: "ThreadListStore",
    state: () => (defaultState),
    getters: {
    },
    actions: {
        getThreadSummary() {
            this.loading_getThreads = true;
            this.result_getThreadsSuccess = false;
            ForumService.getThreadSummary().then((response) => {
                this.threads = response.data;
                this.result_getThreadsSuccess = true;
                this.loading_getThreads = false;
            }, error => {
                this.result_getThreadsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        },
        getThreads() {
            
        },
        createThread(request: CreateThreadRequest) {
            this.loading_createThread = true;
            this.result_createThreadSuccess = false;
            ForumService.createThread(request).then((response) => {
                this.thread = response.data;
                this.result_createThreadSuccess = true;
                this.loading_createThread = false;
            }, error => {
                this.result_createThreadSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            }); 
        },
        createThreadWithStartingPost(request: CreateThreadWithPostRequest) {
            this.loading_createThread = true;
            this.result_createThreadSuccess = false;
            ForumService.createThreadWithStartingPost(request).then((response) => {
                this.thread = response.data;
                this.result_createThreadSuccess = true;
                this.loading_createThread = false;
            }, error => {
                this.result_createThreadSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            }); 
        }
    }
});