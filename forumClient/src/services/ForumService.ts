import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { ForumAppState } from "@/Dto/app/ForumAppState";
import type { MessageBasicInfo } from "@/Dto/app/MessageInfo";
import type { CreateThreadRequest, CreateThreadWithPostRequest, ThreadBasicInfo } from "@/Dto/app/ThreadInfo";
import type { TopicBasicInfo } from "@/Dto/app/TopicInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getForumAppState = async (): Promise<ApiSuccessResponse<ForumAppState>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/state`);
}

const getForumPublicAppState = async (): Promise<ApiSuccessResponse<ForumAppState>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/state`);
}

const getThreadSummary = async (): Promise<ApiSuccessResponse<ThreadBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/threads`);
}

const getThreadsByTopic = async (topicId: string): Promise<ApiSuccessResponse<ThreadBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/threads/${topicId}`);
}

const getThreadPosts = async (threadId: string): Promise<ApiSuccessResponse<MessageBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/threads/${threadId}/posts`);
}

const getTopics = async (): Promise<ApiSuccessResponse<TopicBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/topics`);
}

const getNewestTopics = async (): Promise<ApiSuccessResponse<TopicBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/topics/newest`);
}

const createThread = async (request: CreateThreadRequest): Promise<ApiSuccessResponse<ThreadBasicInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/threads/create`, request);
}

const createThreadWithStartingPost = async (request: CreateThreadWithPostRequest): Promise<ApiSuccessResponse<ThreadBasicInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/threads/createAndPost`, request);
}
export default {
    getForumAppState,
    getForumPublicAppState,
    getThreadSummary,
    getThreadsByTopic,
    getThreadPosts,
    getTopics,
    getNewestTopics,
    createThread,
    createThreadWithStartingPost
}