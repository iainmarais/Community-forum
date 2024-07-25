import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { CreateTopicRequest, TopicBasicInfo, TopicFullInfo } from "@/Dto/app/TopicInfo";
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

export default { 
    getTopicFullInfo,
    createNewTopic,
    getTopicBasicInfo
}