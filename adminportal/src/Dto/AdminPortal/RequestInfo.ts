import type { TriageStatus, TriageType } from "./UpdateRequestTriageStatusRequest";
import type { UserBasicInfo, UserEntry } from "./UserInfo";

export type RequestEntry = {
    requestId: string;
    createdDate: Date;
    supportRequestTitle: string;
    supportRequestContent: string;
    dateUpdated: Date;
    dateResolved: Date;
    isMarkedForDelete: boolean;
    dateMarkedForDelete: Date;
    triageStatus: TriageStatus;
    triageType: TriageType;
    userRequestMappings: UserRequestMappingEntry[];
    createdByUser: UserEntry;
    assignedToUser: UserEntry;
    resolvedByUser: UserEntry;
}

export type UserRequestMappingEntry = {
    UserMappingId: string,
    user: UserEntry,
    userId: string,
    request: RequestEntry,
    requestId: string,
    isCreator: boolean,
    isAssigned: boolean,
    isResolved: boolean,
    isClosed: boolean,
    dateAssigned: Date
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