import { ApiResponse } from './ApiResponse';

export interface IApiErrorResponse {
    statusCode: number,
    statusMessage: string,
    exceptionType: string,
    exceptionMessage: string,
    responseMessage?: string
}

export class ApiErrorResponse extends ApiResponse<null> implements IApiErrorResponse {
    constructor(
        public statusCode: number,
        public statusMessage: string,
        public exceptionType: string,
        public exceptionMessage: string,
        public responseMessage?: string
    ) {
        super(false, responseMessage || exceptionMessage, null, exceptionType);
    }
}