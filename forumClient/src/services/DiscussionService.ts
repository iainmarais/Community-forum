import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { CreatePostRequest, PostFullInfo } from "@/Dto/app/PostInfo";
import type { ThreadFullInfo } from "@/Dto/app/ThreadInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getThreadFullInfo = async (threadId: string): Promise<ApiSuccessResponse<ThreadFullInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/thread/${threadId}/fullinfo`);
}

const postReply = async (request: CreatePostRequest): Promise<ApiSuccessResponse<PostFullInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/thread/${request.threadId}/posts/create`, request);
}

export default {
    getThreadFullInfo,
    postReply
}