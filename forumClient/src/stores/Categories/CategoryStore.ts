import { defineStore } from "pinia";
import type { CategoryBasicInfo, CategoryFullInfo } from "@/Dto/app/CategoryInfo";
import CategoryService from "@/services/CategoryService";
import ErrorHandler from "@/Handlers/ErrorHandler";

type CategoryStoreState = {
    category: CategoryFullInfo,
    categories: CategoryBasicInfo[],
    loading_getCategoryFullInfo: boolean,
    result_getCategoryFullInfoSuccess: boolean,

    loading_getCategories: boolean,
    result_getCategoriesSuccess: boolean
}

const defaultState: CategoryStoreState = {
    category: {} as CategoryFullInfo,
    categories: [],
    loading_getCategoryFullInfo: false,
    result_getCategoryFullInfoSuccess: false,

    loading_getCategories: false,
    result_getCategoriesSuccess: false
}

export const useCategoryStore = defineStore({
    id: "category",
    state: () => (defaultState),
    getters: {

    },
    actions: {
        getCategoryFullInfo(categoryId: string) {
            this.loading_getCategoryFullInfo = true;
            CategoryService.getCategoryFullInfo(categoryId).then(response => {
                this.category = response.data;
                this.result_getCategoryFullInfoSuccess = true;
                this.loading_getCategoryFullInfo = false;
            }, error => {
                this.result_getCategoryFullInfoSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        },
        getCategories() {
            this.loading_getCategories = true;
            CategoryService.getCategories().then(response => {
                this.categories = response.data;
                this.result_getCategoriesSuccess = true;
                this.loading_getCategories = false;
            }, error => {
                this.result_getCategoriesSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        }   
    }
});
    