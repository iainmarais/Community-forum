import { defineStore } from "pinia";
import type { CategoryBasicInfo, CategoryFullInfo, CreateCategoryRequest} from "@/Dto/app/CategoryInfo";
import CategoryService from "@/services/CategoryService";
import ErrorHandler from "@/Handlers/ErrorHandler";

type CategoryStoreState = {
    category: CategoryFullInfo,
    categories: CategoryBasicInfo[],
    loading_getCategoryFullInfo: boolean,
    result_getCategoryFullInfoSuccess: boolean,

    loading_getCategories: boolean,
    result_getCategoriesSuccess: boolean,

    loading_getSelectedCategory: boolean,
    result_getSelectedCategorySuccess: boolean,

    loading_createCategory: boolean,
    result_createCategorySuccess: boolean,

    selectedCategoryId?: string,
    selectedCategory: CategoryBasicInfo,

    loading_selectedCategory: Map<string, boolean>,
}

const defaultState: CategoryStoreState = {
    category: {} as CategoryFullInfo,
    categories: [],
    loading_getCategoryFullInfo: false,
    result_getCategoryFullInfoSuccess: false,

    loading_getCategories: false,
    result_getCategoriesSuccess: false,

    loading_createCategory: false,
    result_createCategorySuccess: false,

    selectedCategory: {} as CategoryBasicInfo,

    loading_getSelectedCategory: false,
    result_getSelectedCategorySuccess: false,

    loading_selectedCategory: new Map<string, boolean>(),

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
        },
        getSelectedCategory(categoryId: string) {
            this.loading_selectedCategory.set(categoryId, true);
            CategoryService.getSelectedCategory(categoryId).then(response => {
                this.selectedCategory = response.data;
                this.result_getSelectedCategorySuccess = true;
                this.loading_selectedCategory.set(categoryId, false);
            }, error => {
                this.result_getSelectedCategorySuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        },        
        createCategory(request: CreateCategoryRequest) { 
            this.loading_createCategory = true;
            this.result_createCategorySuccess = false;
            CategoryService.createCategory(request).then(response => {
                this.categories = response.data;
                this.result_createCategorySuccess = true;
                this.loading_createCategory = false;
            }, error => {
                this.result_createCategorySuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            });
        }
    }
});
    