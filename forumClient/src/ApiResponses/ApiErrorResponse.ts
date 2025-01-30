import { ApiResponse, ApiResponseType, ApiResponseWithMetadata } from './ApiResponse';

export class ApiErrorResponse<D> extends ApiResponse<D> {
    constructor(
        errorMessage: string,
        data?: D,
        errorCode?: string,
        statusCode?: number
    ) {
        super(false, errorMessage, data, ApiResponseType.Error, errorCode, statusCode);
    }
}

export class ApiFailureResponse<D> extends ApiResponse<D> {
    constructor(
        failureMessage: string,
        data?: D,
        errorCode?: string,
        statusCode?: number
    ) {
        super(false, failureMessage, data, ApiResponseType.Failure, errorCode, statusCode);
    }
}

export class ApiErrorResponseWithMetadata<D, M> extends ApiResponseWithMetadata<D, M> {
    constructor(
        errorMessage: string,
        data: D | undefined,
        metadata: M,
        errorCode?: string,
        statusCode?: number
    ) {
        super(false, errorMessage, data, metadata, ApiResponseType.Error, errorCode, statusCode);
    }
}

export class ApiFailureResponseWithMetadata<D, M> extends ApiResponseWithMetadata<D, M> {
    constructor(
        failureMessage: string,
        data: D | undefined,
        metadata: M,
        errorCode?: string,
        statusCode?: number
    ) {
        super(false, failureMessage, data, metadata, ApiResponseType.Failure, errorCode, statusCode);
    }
}

export class ApiClientErrorResponses {
    static withData<D>(
        errorMessage: string,
        data: D | undefined,
        errorCode?: string,
        statusCode?: number
    ): ApiErrorResponse<D> {
        return new ApiErrorResponse(errorMessage, data, errorCode, statusCode);
    }

    static withDataAndMetadata<D, M>(
        errorMessage: string,
        data: D | undefined,
        metadata: M,
        errorCode?: string,
        statusCode?: number
    ): ApiErrorResponseWithMetadata<D, M> {
        return new ApiErrorResponseWithMetadata(errorMessage, data, metadata, errorCode, statusCode);
    }

    static withoutData(
        errorMessage: string,
        errorCode?: string,
        statusCode?: number
    ): ApiErrorResponse<unknown> {
        return new ApiErrorResponse(errorMessage, undefined, errorCode, statusCode);
    }
}

export class ApiFailureResponses {
    static withData<D>(
        failureMessage: string,
        data: D | undefined,
        errorCode?: string,
        statusCode?: number
    ): ApiFailureResponse<D> {
        return new ApiFailureResponse(failureMessage, data, errorCode, statusCode);
    }

    static withDataAndMetadata<D, M>(
        failureMessage: string,
        data: D | undefined,
        metadata: M,
        errorCode?: string,
        statusCode?: number
    ): ApiFailureResponseWithMetadata<D, M> {
        return new ApiFailureResponseWithMetadata(failureMessage, data, metadata, errorCode, statusCode);
    }

    static withoutData(
        failureMessage: string,
        errorCode?: string,
        statusCode?: number
    ): ApiFailureResponse<unknown> {
        return new ApiFailureResponse(failureMessage, undefined, errorCode, statusCode);
    }
}