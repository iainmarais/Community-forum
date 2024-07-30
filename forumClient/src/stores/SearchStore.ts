import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { TopicBasicInfo, TopicSummary } from "@/Dto/app/TopicInfo";
import TopicService from "@/services/TopicService";
import { defineStore } from "pinia";
import ErrorHandler from "@/Handlers/ErrorHandler";

type SearchStoreState = {
    topicSearchResults: PaginatedData<TopicBasicInfo[], TopicSummary>,
    searchResults: PaginatedData<any, any>,

    rowsPerPage: number,
    currentPageNumber: number,
    searchQuery?: string,

    loading_search: boolean,
    result_searchSuccess: boolean
}

export const useSearchStore = defineStore({
    id: "search",
    state: (): SearchStoreState => ({
        searchResults: {
            rows: [] ,
            pageNumber: 0,
            totalPages: 0,
            totalRows: 0,
            summary: {}
        },

        topicSearchResults: {
            rows: [] as TopicBasicInfo[],
            pageNumber: 0,
            totalPages: 0,
            totalRows: 0,
            summary: {
                totalTopics: 0
            }
        },

        searchQuery: "",
        rowsPerPage: 10,
        currentPageNumber: 1,

        loading_search: false,
        result_searchSuccess: false
    }),
    getters: {

    },
    actions: {
        searchTopics(searchTerm: string) {
            this.searchQuery = searchTerm;
            this.loading_search = true;
            this.result_searchSuccess = false;
            TopicService.getTopics(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then((response) => {
                this.topicSearchResults = response.data;
                this.loading_search = false;
                this.result_searchSuccess = true;
            }, error => {
                this.result_searchSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);  // Improved error logging
            })

        },
        searchPosts(searchTerm: string) {},
        searchUsers(searchTerm: string) {},
        searchBoards(searchTerm: string) {},
        searchImages(searchTerm: string) {}
    }
})