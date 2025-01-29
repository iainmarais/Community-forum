import type { PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { CreateAdminPortalRequest } from "@/Dto/AdminPortal/CreateAdminPortalRequest";
import type { RequestBasicInfo, RequestSummary } from "@/Dto/AdminPortal/RequestInfo"
import ErrorHandler from "@/Handlers/ErrorHandler";
import RequestService from "@/Services/RequestService";
import { defineStore } from "pinia";
import { useToast } from "vue-toastification";

const toast = useToast();

type ServiceRequestsStore = {
    loading_createServiceRequest: boolean;
    result_createServiceRequestSuccess:  boolean;

    loading_getRequests: boolean;
    result_getRequestsSuccess: boolean;

    requests: PaginatedData<RequestBasicInfo[], RequestSummary>;

    searchQuery?: string;

    currentPageNumber: number;
    rowsPerPage: number;
}

const defaultState: ServiceRequestsStore = {
    loading_createServiceRequest: false,
    result_createServiceRequestSuccess: false,

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
    },
    currentPageNumber: 1,
    rowsPerPage: 10,
}

export const useServiceRequestsStore = defineStore({
    id: "SupportRequestsStore",
    state: () => defaultState,
    getters: {},
    actions: {
        getSupportRequests() {
                this.loading_getRequests = true;
                this.result_getRequestsSuccess = false;
            RequestService.getSupportRequests(this.currentPageNumber, this.rowsPerPage, this.searchQuery).then(response => {
                this.loading_getRequests = false;
                this.result_getRequestsSuccess = true;
                this.requests = response.data;
            }, error => {
                this.loading_getRequests = false;
                this.result_getRequestsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            });
        },
        createServiceRequest(req: CreateAdminPortalRequest) {
            this.loading_createServiceRequest = true;
            this.result_createServiceRequestSuccess = false;
            RequestService.createAdminPortalRequest(req).then(response => {
                this.loading_createServiceRequest = false;
                this.result_createServiceRequestSuccess = true;
                toast.successful(`Service request ${response.data.request.serviceRequestTitle} created successfully.`);
            }, error => {
                this.loading_createServiceRequest = false;
                this.result_getRequestsSuccess = false;
                ErrorHandler.handleApiErrorResponse(error);
            })
        }
    }
});