import type { ApiSuccessResponse, PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { BoardBasicInfo, BoardFullInfo, BoardSummary } from "@/Dto/app/BoardInfo";
import type { CategoryBasicInfo, CategoryFullInfo, CreateCategoryRequest } from "@/Dto/app/CategoryInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getCategories = async (): Promise<ApiSuccessResponse<CategoryBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/categories`);
}

const getSelectedCategoryBasicInfo = async (categoryId: string): Promise<ApiSuccessResponse<CategoryBasicInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/categories/${categoryId}`);
}

const getSelectedCategory = async (categoryId: string): Promise<ApiSuccessResponse<CategoryFullInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/categories/${categoryId}/fullinfo`);
}

const createCategory = async (request: CreateCategoryRequest): Promise<ApiSuccessResponse<CategoryBasicInfo[]>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/categories/create`, request);
}

const getBoardsForCategory = async (boardId: string): Promise<ApiSuccessResponse<PaginatedData<BoardBasicInfo[], BoardSummary>>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/categories/${boardId}/boards`);
}



export default { 
    getCategories, 
    getSelectedCategoryBasicInfo,
    getSelectedCategory,
    createCategory
}