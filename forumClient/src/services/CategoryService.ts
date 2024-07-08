import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { CategoryBasicInfo, CategoryFullInfo } from "@/Dto/app/CategoryInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getCategories = async (): Promise<ApiSuccessResponse<CategoryBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/categories`);
}

const getCategoryFullInfo = async (categoryId: string): Promise<ApiSuccessResponse<CategoryFullInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/categories/${categoryId}/fullinfo`);
}


export default { 
    getCategories, 
    getCategoryFullInfo
}