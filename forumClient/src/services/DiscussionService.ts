import type { ApiSuccessResponse, PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { CreatePostRequest, PostFullInfo, PostSummary } from "@/Dto/app/PostInfo";
import type { ThreadFullInfo } from "@/Dto/app/ThreadInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getThreadFullInfo = async (threadId: string): Promise<ApiSuccessResponse<ThreadFullInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/thread/${threadId}/fullinfo`);
}

const postReply = async (request: CreatePostRequest): Promise<ApiSuccessResponse<PaginatedData<PostFullInfo[], PostSummary>>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/thread/${request.threadId}/posts/create`, request);
}

const getPostsForThread = async (threadId: string, pageNumber: number, rowsPerPage: number, searchTerm?: string): Promise<ApiSuccessResponse<PaginatedData<PostFullInfo[], PostSummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if(searchTerm) {
        queryParams.push(`searchTerm=${searchTerm}`);
    }
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/thread/${threadId}/posts?${queryParams.join('&')}`);
}

export default {
    getThreadFullInfo,
    postReply,
    getPostsForThread
}