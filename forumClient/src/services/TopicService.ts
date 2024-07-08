import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { TopicFullInfo } from "@/Dto/app/TopicInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getTopicFullInfo = async (topicId: string): Promise<ApiSuccessResponse<TopicFullInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/topics/${topicId}/fullinfo`);
}

export default { 
    getTopicFullInfo 
}