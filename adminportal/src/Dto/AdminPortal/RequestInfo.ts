import type { TriageStatus, TriageType } from "./UpdateRequestTriageStatusRequest";
import type { UserBasicInfo, UserEntry } from "./UserInfo";

export type RequestEntry = {
    requestId: string,
    createdByUserId: string,
    createdDate: Date,
    supportRequestTitle: string,
    supportRequestContent: string,
    assignedToUserId: string | null,
    resolvedByUserId: string | null,
    lastUpdatedByUserId: string | null,
    isResolved: boolean,
    dateUpdated: Date | null,
    dateResolved: Date | null,
    triageStatus: TriageStatus,
    triageType: TriageType,
    createdByUser: UserEntry,
    assignedToUser: UserEntry,
    resolvedByUser: UserEntry,
    lastUpdatedByUser: UserEntry
}

export type RequestBasicInfo = {
    request: RequestEntry,
    createdByUser: UserBasicInfo,
}

export type RequestSummary = {
    totalRequests: number,
    numResolvedRequests: number,
    numAssignedRequests: number,
    numPendingRequests: number,   
}