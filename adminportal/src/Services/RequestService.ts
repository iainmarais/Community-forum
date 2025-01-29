import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";
import { ApiSuccessResponse, type PaginatedData } from "@/ApiResponses/ApiSuccessResponse";
import type { RequestBasicInfo, RequestSummary } from "@/Dto/AdminPortal/RequestInfo";
import type { CreateAdminPortalRequest } from "@/Dto/AdminPortal/CreateAdminPortalRequest";

//Create a new support request
const createAdminPortalRequest = async (req: CreateAdminPortalRequest) :Promise<ApiSuccessResponse<RequestBasicInfo>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/supportrequests/create`, req);
}

const getSupportRequests = async (pageNumber: number, rowsPerPage: number, searchTerm?: string): Promise<ApiSuccessResponse<PaginatedData<RequestBasicInfo[],RequestSummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if (searchTerm) {
        queryParams.push(`searchTerm=${encodeURIComponent(searchTerm)}`);
    }
    return await AxiosClient.Get<PaginatedData<RequestBasicInfo[], RequestSummary>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/supportrequests?${queryParams.join('&')}`);
}

const getSupportRequestsByUser = (userId: string, pageNumber: number, rowsPerPage: number, searchTerm?: string) : Promise<ApiSuccessResponse<PaginatedData<RequestBasicInfo[],RequestSummary>>> => {
    const queryParams = [];
    queryParams.push(`pageNumber=${pageNumber}`);
    queryParams.push(`rowsPerPage=${rowsPerPage}`);
    if (searchTerm) {
        queryParams.push(`searchTerm=${encodeURIComponent(searchTerm)}`);
    }
    return AxiosClient.Get<PaginatedData<RequestBasicInfo[], RequestSummary>>(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/adminportal/supportrequests/${userId}?${queryParams.join('&')}`);
}

export default {
    createAdminPortalRequest,
    getSupportRequests,
    getSupportRequestsByUser
}
