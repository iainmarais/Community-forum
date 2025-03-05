import { ApiResponse, ApiResponseType, ApiResponseWithMetadata } from './ApiResponse';
import type { AllowedUserActions } from '@/Actions/AllowedUserActions';

export class ApiSuccessResponse<D> extends ApiResponse<D> {
    constructor(
        message: string,
        data: D | undefined,
        public allowedUserActions?: AllowedUserActions,
        statusCode?: number
    ) {
        super(true, message, data, ApiResponseType.Success, undefined, statusCode);
        this.allowedUserActions = allowedUserActions;
    }
}

export class ApiSuccessResponseWithMetadata<D, M> extends ApiResponseWithMetadata<D, M> {
    constructor(
        message: string,
        data: D | undefined,
        metadata: M,
        public allowedUserActions?: AllowedUserActions,
        statusCode?: number
    ) {
        super(true, message, data, metadata, ApiResponseType.Success, undefined, statusCode);
        this.allowedUserActions = allowedUserActions;
    }
}

export class ApiSuccessResponses {
    static withData<D>(
        message: string,
        data: D,
        allowedUserActions?: AllowedUserActions,
        statusCode?: number
    ): ApiSuccessResponse<D> {
        return new ApiSuccessResponse(message, data, allowedUserActions, statusCode);
    }

    static withDataAndMetadata<D, M>(
        message: string,
        data: D,
        metadata: M,
        allowedUserActions?: AllowedUserActions,
        statusCode?: number
    ): ApiSuccessResponseWithMetadata<D, M> {
        return new ApiSuccessResponseWithMetadata(message, data, metadata, allowedUserActions, statusCode);
    }

    static withoutData(
        message: string,
        statusCode?: number
    ): ApiSuccessResponse<unknown> {
        return new ApiSuccessResponse(message, null, undefined, statusCode);
    }
}

export interface PaginatedData<D, S = unknown> {
    rows: D;
    pageNumber: number;
    rowsPerPage: number;
    totalPages: number;
    summary?: S;
}