import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { CreateGalleryItemRequest, GalleryItemBasicInfo } from "@/Dto/app/GalleryItemInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";
import type { AxiosRequestHeaders } from "axios";
import { type ApiFileResponse } from "@/ApiResponses/ApiFileResponse";

const getGalleryItems = async (): Promise<ApiSuccessResponse<GalleryItemBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/gallery/items`);
}

const getSelectedGalleryItem = async (galleryItemId: string): Promise<ApiSuccessResponse<GalleryItemBasicInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/gallery/${galleryItemId}`);
}

const createGalleryItem = async (formData: FormData): Promise<ApiSuccessResponse<GalleryItemBasicInfo[]>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/gallery/items/create`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        } as AxiosRequestHeaders
    });
}

const getImage = async (fileName: string): Promise<ApiSuccessResponse<ApiFileResponse>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/gallery/${fileName}`);
}

export default {
    getGalleryItems,
    getSelectedGalleryItem,
    createGalleryItem,
    getImage
}