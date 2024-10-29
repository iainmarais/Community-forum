import { ApiSuccessResponse, type PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import type { BoardBasicInfo, BoardSummary } from "@/Dto/AdminPortal/BoardInfo";
import type { CategoryBasicInfo, CategorySummary } from "@/Dto/AdminPortal/CategoryInfo";
import type { CreateBoardRequest } from "@/Dto/AdminPortal/CreateBoardRequest";
import type { CreateCategoryRequest } from "@/Dto/AdminPortal/CreateCategoryRequest";
import type { CreateTopicRequest } from "@/Dto/AdminPortal/CreateTopicRequest";
import type { PostBasicInfo, PostSummary } from "@/Dto/AdminPortal/PostInfo";
import type { TopicBasicInfo, TopicSummary } from "@/Dto/AdminPortal/TopicInfo";
import AxiosClient from "@/http/AxiosClient";

const createCategory = async (request: CreateCategoryRequest) : Promise<ApiSuccessResponse<object>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/categories/create`, request);
}

const createBoard = async (request: CreateBoardRequest) : Promise<ApiSuccessResponse<object>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/boards/create`, request);
}

const createTopic = async (request: CreateTopicRequest) : Promise<ApiSuccessResponse<object>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/topics/create`, request);
}

const getCategories = async (pageNumber: number, rowsPerPage: number, searchTerm?: string): Promise<ApiSuccessResponse<PaginatedData<CategoryBasicInfo[], CategorySummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if(searchTerm) {
        queryParams.push(`searchTerm=${searchTerm}`);
    }
    var res = await AxiosClient.Get<PaginatedData<CategoryBasicInfo[], CategorySummary>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/categories?${queryParams.join('&')}`);
    return res;
}

const getBoards = async (pageNumber: number, rowsPerPage: number, searchTerm?: string): Promise<ApiSuccessResponse<PaginatedData<BoardBasicInfo[], BoardSummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if(searchTerm) {
        queryParams.push(`searchTerm=${searchTerm}`);
    }
    var res = await AxiosClient.Get<PaginatedData<BoardBasicInfo[], BoardSummary>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/boards?${queryParams.join('&')}`);
    return res;
}

const getTopics = async (pageNumber: number, rowsPerPage: number, searchTerm?: string): Promise<ApiSuccessResponse<PaginatedData<TopicBasicInfo[], TopicSummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if(searchTerm) {
        queryParams.push(`searchTerm=${searchTerm}`);
    }
    var res = await AxiosClient.Get<PaginatedData<TopicBasicInfo[], TopicSummary>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/topics?${queryParams.join('&')}`);
    return res;
}

const getPosts = async (pageNumber: number, rowsPerPage: number, searchTerm?: string): Promise<ApiSuccessResponse<PaginatedData<PostBasicInfo[], PostSummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if(searchTerm) {
        queryParams.push(`searchTerm=${searchTerm}`);
    }
    var res = await AxiosClient.Get<PaginatedData<PostBasicInfo[], PostSummary>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/posts?${queryParams.join('&')}`);
    return res;
}

const deleteCategory = async(categoryId: string): Promise<ApiSuccessResponse<object>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/categories/${categoryId}/delete`, null)
}

const deleteBoard = async (boardId: string): Promise<ApiSuccessResponse<object>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/boards/${boardId}/delete`, null);
}

const deleteTopic = async (topicId: string): Promise<ApiSuccessResponse<object>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/topics/${topicId}/delete`, null);
}


export default {
    createCategory,
    createBoard,
    createTopic,
    getCategories,
    getBoards,
    getTopics,
    getPosts,
    deleteCategory,
    deleteBoard,
    deleteTopic
}