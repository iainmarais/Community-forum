import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { BoardBasicInfo, BoardSummary } from "@/Dto/AdminPortal/BoardInfo";
import type { CategoryBasicInfo, CategorySummary } from "@/Dto/AdminPortal/CategoryInfo";
import type { CreateBoardRequest } from "@/Dto/AdminPortal/CreateBoardRequest";
import type { CreateCategoryRequest } from "@/Dto/AdminPortal/CreateCategoryRequest";
import type { CreateTopicRequest } from "@/Dto/AdminPortal/CreateTopicRequest";
import type { PostBasicInfo, PostSummary } from "@/Dto/AdminPortal/PostInfo";
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
    posts: PaginatedData<PostBasicInfo[], PostSummary>;

    //Get results
    result_getCategories: boolean;
    result_getBoards: boolean;
    result_getTopics: boolean;
    result_getPosts: boolean;

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

    //Posts
    result_createPost: boolean;
    result_updatePost: boolean;
    result_deletePost: boolean;
}

const defaultState: ContentManagementStore = {
    loading: false,

    currentPageNumber: 1,
    rowsPerPage: 10,

    categories: {} as PaginatedData<CategoryBasicInfo[], CategorySummary>,
    boards: {} as PaginatedData<BoardBasicInfo[], BoardSummary>,
    topics: {} as PaginatedData<TopicBasicInfo[], TopicSummary>,
    posts: {} as PaginatedData<PostBasicInfo[], PostSummary>,

    //Get results
    result_getCategories: false,
    result_getBoards: false,
    result_getTopics: false,
    result_getPosts: false,
    
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

    //Posts
    result_createPost: false,
    result_updatePost: false,
    result_deletePost: false,
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
            ContentManagementService.getCategories(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading=false;
                this.categories = response.data;
            }, error => {
                this.loading = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        getBoards () {
            this.loading = true;
            ContentManagementService.getBoards(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading=false;
                this.boards = response.data;
            }, error => {
                this.loading = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        getTopics () {
            this.loading = true;
            ContentManagementService.getTopics(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading = false,
                this.topics = response.data;
            }, error => {
                this.loading = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        getPosts () {
            this.loading = true;
            ContentManagementService.getPosts(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading = false,
                this.posts = response.data;
            }, error => {
                this.loading = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },        
        deleteCategory (categoryId: string) {
            this.loading = true;
            this.result_deleteCategory = false;
            ContentManagementService.deleteCategory(categoryId).then(response => {
                this.loading = false;
                this.result_deleteCategory = true;
            }, error => {
                this.loading = false;
                this.result_deleteCategory = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
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