import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { BoardBasicInfo, BoardSummary } from "@/Dto/AdminPortal/BoardInfo";
import type { CategoryBasicInfo, CategorySummary } from "@/Dto/AdminPortal/CategoryInfo";
import type { CreateBoardRequest } from "@/Dto/AdminPortal/CreateBoardRequest";
import type { CreateCategoryRequest } from "@/Dto/AdminPortal/CreateCategoryRequest";
import type { CreateTopicRequest } from "@/Dto/AdminPortal/CreateTopicRequest";
import type { TopicBasicInfo, TopicSummary } from "@/Dto/AdminPortal/TopicInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import ContentManagementService from "@/Services/ContentManagementService";
import { defineStore } from "pinia";

type ContentManagementStore = {
    loading: boolean;

    searchQuery?: string;
    currentPageNumber: number;
    rowsPerPage: number;

    categories: PaginatedData<CategoryBasicInfo[], CategorySummary>;
    boards: PaginatedData<BoardBasicInfo[], BoardSummary>;
    topics: PaginatedData<TopicBasicInfo[], TopicSummary>;

    //Get results
    result_getCategories: boolean;
    result_getBoards: boolean;
    result_getTopics: boolean;

    //Categories
    result_createCategory: boolean;
    result_updateCategory: boolean;
    result_deleteCategory: boolean;

    //Boards
    result_createBoard: boolean;
    result_updateBoard: boolean;
    result_deleteBoard: boolean;

    //Topics
    result_createTopic: boolean;
    result_updateTopic: boolean;
    result_deleteTopic: boolean;
}

const defaultState: ContentManagementStore = {
    loading: false,

    currentPageNumber: 1,
    rowsPerPage: 10,

    categories: {} as PaginatedData<CategoryBasicInfo[], CategorySummary>,
    boards: {} as PaginatedData<BoardBasicInfo[], BoardSummary>,
    topics: {} as PaginatedData<TopicBasicInfo[], TopicSummary>,

    //Get results
    result_getCategories: false,
    result_getBoards: false,
    result_getTopics: false,
    
    //Categories
    result_createCategory: false,
    result_updateCategory: false,
    result_deleteCategory: false,

    //Boards
    result_createBoard: false,
    result_updateBoard: false,
    result_deleteBoard: false,

    //Topics
    result_createTopic: false,
    result_updateTopic: false,
    result_deleteTopic: false,    
}

export const useContentManagementStore = defineStore({
    id: "ContentManagementStore",
    state: () => (defaultState),
    getters: {},
    actions: {

        //Categories
        createCategory (request: CreateCategoryRequest) {
            this.loading = true;
            ContentManagementService.createCategory(request).then(response => {
                this.loading = false;
                this.result_createCategory = true;
            }, error => {
                this.loading = false;
                this.result_createCategory = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        //Boards
        createBoard (request: CreateBoardRequest) {
            this.loading = true;
            ContentManagementService.createBoard(request).then(response => {
                this.loading = false;
                this.result_createBoard = true;
            }, error => {
                this.loading = false;
                this.result_createBoard = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        //Topics
        createTopic (request: CreateTopicRequest) {
            this.loading = true;
            ContentManagementService.createTopic(request).then(response => {
                this.loading = false;
                this.result_createTopic = true;
            }, error => {
                this.loading = false;
                this.result_createTopic = false;
                ErrorHandler.handleApiErrorResponse(error);
            });            
        },
        getCategories () {
            this.loading = true;
            this.result_getCategories = false;
            ContentManagementService.getCategories(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading=false,
                this.result_getCategories = true;
                this.categories = response.data;
            }, error => {
                this.loading = false;
                this.result_getCategories = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        getBoards () {
            this.loading = true;
            this.result_getBoards = false;
            ContentManagementService.getBoards(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading=false,
                this.result_getBoards = true;
                this.boards = response.data;
            }, error => {
                this.loading = false;
                this.result_getBoards = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        getTopics () {
            this.loading = true;
            this.result_getTopics = false;
            ContentManagementService.getTopics(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading = false,
                this.result_getTopics = true;
                this.topics = response.data;
            }, error => {
                this.loading = false;
                this.result_getTopics = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        deleteBoard (boardId: string) {
            this.loading = true;
            this.result_deleteBoard = false;
            ContentManagementService.deleteBoard(boardId).then(response => {
                this.loading = false;
                this.result_deleteBoard = true;
            }, error => {
                this.loading = false;
                this.result_deleteBoard = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        },
        deleteTopic (topicId: string) {
            this.loading = true;
            this.result_deleteTopic = false;
            ContentManagementService.deleteTopic(topicId).then(response => {
                this.loading = false;
                this.result_deleteTopic = true;
            }, error => {
                this.loading = false;
                this.result_deleteTopic = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        }
    }
})