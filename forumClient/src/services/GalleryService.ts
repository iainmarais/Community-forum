import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { GalleryItemBasicInfo } from "@/Dto/app/GalleryItemInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getGalleryItems = async (): Promise<ApiSuccessResponse<GalleryItemBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/gallery/items`);
}

const getSelectedGalleryItem = async (galleryItemId: string): Promise<ApiSuccessResponse<GalleryItemBasicInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/gallery/${galleryItemId}`);
}

export default {
    getGalleryItems,
    getSelectedGalleryItem
}