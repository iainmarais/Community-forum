import type { ApiSuccessResponse, PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
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

const getTopics = async (pageNumber: number, rowsPerPage: number, searchQuery?: string): Promise<ApiSuccessResponse<PaginatedData<TopicBasicInfo[], TopicSummary>>> => {
    const queryParams = [];
    if (searchQuery) {
        queryParams.push(`searchQuery=${searchQuery}`);
    }
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/topics?${queryParams.join("&")}`);
}


export default { 
    getTopicFullInfo,
    createNewTopic,
    getTopicBasicInfo,
    getTopics
}