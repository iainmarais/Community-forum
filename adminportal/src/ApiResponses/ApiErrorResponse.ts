export type ApiErrorResponse = {
    statusCode: number,
    statusMessage: string,
    exceptionType: string,
    exceptionMessage: string,
    responseMessage?: string
}