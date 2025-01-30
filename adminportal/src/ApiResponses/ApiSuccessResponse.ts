import type { AllowedUserActions } from '@/Actions/AllowedUserActions';
import { ApiResponse, ApiResponseWithMetadata } from './ApiResponse';

export interface IApiSuccessResponse<T, M = any | undefined> {
    message: string,
    data: T,
    metaData: M,
    actions?: AllowedUserActions
}

export class ApiSuccessResponse<T, M = any | undefined> extends ApiResponseWithMetadata<T, M> implements IApiSuccessResponse<T, M> {
    public readonly data: T;

    constructor(
        message: string, 
        data: T, 
        public metaData: M, 
        public actions?: AllowedUserActions
    ) {
        super(true, message, data, metaData);
        this.data = data; // Ensure data is set as a required property
    }
}

export type PaginatedData<D, S> = {
    rows: D,
    pageNumber: number,
    totalPages: number,
    totalRows: number,
    summary: S
}