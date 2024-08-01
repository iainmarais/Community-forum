import type { ApiSuccessResponse, PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { ThreadBasicInfo, ThreadSummary } from "@/Dto/app/ThreadInfo";
import type { CreateTopicRequest, TopicBasicInfo, TopicFullInfo, TopicSummary } from "@/Dto/app/TopicInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getTopicFullInfo = async (topicId: string): Promise<ApiSuccessResponse<TopicFullInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/topics/${topicId}/fullinfo`);
}

const createNewTopic = async (request: CreateTopicRequest): Promise<ApiSuccessResponse<TopicFullInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/topics/create`, request);
}

const getTopicBasicInfo = async (topicId: string): Promise<ApiSuccessResponse<TopicBasicInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/topics/${topicId}/basicinfo`);
}

const getThreadsForTopic = async (topicId: string, pageNumber: number, rowsPerPage: number, searchTerm?: string): Promise<ApiSuccessResponse<PaginatedData<ThreadBasicInfo[],ThreadSummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if(searchTerm) {
        queryParams.push(`searchTerm=${searchTerm}`);
    }
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/topics/${topicId}/threads?${queryParams.join('&')}`);
}

export default { 
    getTopicFullInfo,
    createNewTopic,
    getTopicBasicInfo,
    getThreadsForTopic
}