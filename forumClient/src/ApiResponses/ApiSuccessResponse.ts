import type { AllowedUserActions } from '@/Actions/AllowedUserActions';

export interface IApiSuccessResponse<T, M = any | undefined> {
    message: string,
    data: T,
    metaData: M,
    actions?: AllowedUserActions
}

export class ApiSuccessResponse<T, M = any | undefined> implements ApiSuccessResponse<T, M> {
    constructor(public message: string, public data: T, public metaData: M, public actions?: AllowedUserActions) { }
}

export type PaginatedData<D, S> = {
    rows: D,
    pageNumber: number,
    totalPages: number,
    totalRows: number,
    summary: S
}