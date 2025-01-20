import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { RequestBasicInfo, RequestSummary } from "@/Dto/AdminPortal/RequestInfo";
import type { SupportRequest } from "@/Dto/AdminPortal/SupportRequest";
import ErrorHandler from "@/Handlers/ErrorHandler";
import RequestService from "@/Services/RequestService";
import { defineStore } from "pinia";

type SupportRequestsStore = {
    loading_getRequests: boolean;
    result_getRequestsSuccess: boolean;

    requests: PaginatedData<RequestBasicInfo[], RequestSummary>
}

const defaultState: SupportRequestsStore = {
    loading_getRequests: false,
    result_getRequestsSuccess: false,

    requests: {
        rows: [] as RequestBasicInfo[],
        pageNumber: 0,
        totalPages: 0,
        totalRows: 0,
        summary: {
            numAssignedRequests: 0,
            numPendingRequests: 0,
            numResolvedRequests: 0,
            totalRequests: 0
        }
    }
}

export const useSupportRequestsStore = defineStore({
    id: "SupportRequestsStore",
    state: () => defaultState,
    getters: {},
    actions: {
        getSupportRequests() {
            RequestService.getSupportRequests(this.requests.pageNumber, this.requests.rowsPerPage).then(response => {
                this.loading_getRequests = false;
                this.result_getRequestsSuccess = true;
                this.requests = response.data;
            }, error => {
                this.loading_getRequests = false;
                this.result_getRequestsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        }
    }
});