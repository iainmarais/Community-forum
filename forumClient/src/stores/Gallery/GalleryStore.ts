import type { CreateGalleryItemRequest, GalleryItemBasicInfo } from "@/Dto/app/GalleryItemInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import GalleryService from "@/services/GalleryService";
import { defineStore } from "pinia";
import { type ApiFileResponse } from "@/ApiResponses/ApiFileResponse";

type GalleryStoreState = {
    galleryItems?: GalleryItemBasicInfo[],
    selectedGalleryItem?: GalleryItemBasicInfo,

    imageFile?: ApiFileResponse,

    loading_galleryItems: boolean,
    result_galleryItemsSuccess: boolean

    loading_selectedGalleryItem: boolean,
    result_selectedGalleryItemSuccess: boolean,

    loading_createGalleryItem: boolean,
    result_createGalleryItemSuccess: boolean,

    loading_getImage: boolean,
    result_getImageSuccess: boolean
}

const defaultState: GalleryStoreState = {
    galleryItems: [],
    selectedGalleryItem: {} as GalleryItemBasicInfo,

    loading_galleryItems: false,
    result_galleryItemsSuccess: false,

    loading_selectedGalleryItem: false,
    result_selectedGalleryItemSuccess: false,

    loading_createGalleryItem: false,
    result_createGalleryItemSuccess: false,

    loading_getImage: false,
    result_getImageSuccess: false,

    imageFile: {} as ApiFileResponse
}

export const useGalleryStore = defineStore({
    id: "GalleryStore",
    state: () => (defaultState),
    getters: {
    },
    actions: {

        getGalleryItems() {
            this.loading_galleryItems = true;
            this.result_galleryItemsSuccess = false;
            GalleryService.getGalleryItems().then((response) => {
                this.galleryItems = response.data;
                this.result_galleryItemsSuccess = true;
                this.loading_galleryItems = false;
            }, error => {
                this.result_galleryItemsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        },

        getSelectedGalleryItem(galleryItemId: string) {
            this.loading_selectedGalleryItem = true;
            this.result_selectedGalleryItemSuccess = false;
            GalleryService.getSelectedGalleryItem(galleryItemId).then((response) => {
                this.selectedGalleryItem = response.data;
                this.result_selectedGalleryItemSuccess = true;
                this.loading_selectedGalleryItem = false;
            }, error => {
                this.result_selectedGalleryItemSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        },

        createGalleryItem(formData: FormData) {
            this.loading_createGalleryItem = true;
            this.result_createGalleryItemSuccess = false;
            GalleryService.createGalleryItem(formData).then((response) => {
                this.getGalleryItems();
                this.result_createGalleryItemSuccess = true;
                this.loading_createGalleryItem = false;
            }, error => {
                this.result_createGalleryItemSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        },

        getImage(fileName: string) {
            this.loading_getImage = true;
            this.result_getImageSuccess = false;

            GalleryService.getImage(fileName).then((response) => {
                this.imageFile = response.data;
                this.result_getImageSuccess = true;
                this.loading_getImage = false;
            }, error => {
                this.result_getImageSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        
        }
    }
});