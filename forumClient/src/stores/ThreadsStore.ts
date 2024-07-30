import type { CreateThreadRequest, CreateThreadWithPostRequest, ThreadBasicInfo } from "@/Dto/app/ThreadInfo";
import type { TopicBasicInfo } from "@/Dto/app/TopicInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import ForumService from "@/services/ForumService";
import TopicService from "@/services/TopicService";
import { defineStore } from "pinia";

type ThreadsStoreState = {
    threads: ThreadBasicInfo[],
    associatedTopic: TopicBasicInfo,

    loading_getAssociatedTopic: boolean,
    result_getAssociatedTopicSuccess: boolean,

    loading_getThreads: boolean,
    result_getThreadsSuccess: boolean,

    thread: ThreadBasicInfo | undefined,
    loading_createThread: boolean,
    result_createThreadSuccess: boolean
}

const defaultState: ThreadsStoreState = {
    threads: [] as ThreadBasicInfo[],
    associatedTopic: {} as TopicBasicInfo,

    loading_getAssociatedTopic: false,
    result_getAssociatedTopicSuccess: false,

    loading_getThreads: false,
    result_getThreadsSuccess: false,

    thread: undefined,
    loading_createThread: false,
    result_createThreadSuccess: false
}

export const useThreadsStore = defineStore({
    id: "ThreadsStore",
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
        },
        getAssociatedTopic(threadId: string) {
            this.loading_getAssociatedTopic = true;
            this.result_getAssociatedTopicSuccess = false;
            TopicService.getTopicBasicInfo(threadId).then((response) => {
                this.associatedTopic = response.data;
                this.result_getAssociatedTopicSuccess = true;
                this.loading_getAssociatedTopic = false;
            }, error => {
                this.result_getAssociatedTopicSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        }
    }
});