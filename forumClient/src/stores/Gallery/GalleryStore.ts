import type { GalleryItemBasicInfo } from "@/Dto/app/GalleryItemInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import GalleryService from "@/services/GalleryService";
import { defineStore } from "pinia";

type GalleryStoreState = {
    galleryItems: GalleryItemBasicInfo[],
    selectedGalleryItem: GalleryItemBasicInfo,

    loading_galleryItems: boolean,
    result_galleryItemsSuccess: boolean

    loading_selectedGalleryItem: boolean,
    result_selectedGalleryItemSuccess: boolean
}

const defaultState: GalleryStoreState = {
    galleryItems: [],
    selectedGalleryItem: {} as GalleryItemBasicInfo,

    loading_galleryItems: false,
    result_galleryItemsSuccess: false,

    loading_selectedGalleryItem: false,
    result_selectedGalleryItemSuccess: false
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
        }
    }
});