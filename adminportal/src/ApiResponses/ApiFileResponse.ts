import { ApiResponse } from './ApiResponse';

export interface IApiFileResponse {
    responseMessage: string | null,
    fileContents: string,
    contentType: string,
    fileDownloadName: string,
    fileName: string
}

export class ApiFileResponse extends ApiResponse<string> implements IApiFileResponse {
    constructor(
        public responseMessage: string | null,
        public fileContents: string,
        public contentType: string,
        public fileDownloadName: string,
        public fileName: string
    ) {
        super(true, responseMessage || 'File retrieved successfully', fileContents);
    }
}
