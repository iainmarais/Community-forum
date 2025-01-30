import { ApiResponse, ApiResponseType } from './ApiResponse';

export class ApiFileResponse extends ApiResponse<Uint8Array> {
    public contentType: string = '';
    public fileDownloadName: string = '';
    public fileName: string = '';

    constructor(
        isSuccessful: boolean,
        message: string,
        fileContents?: Uint8Array,
        contentType: string = '',
        fileDownloadName: string = '',
        fileName: string = '',
        errorCode?: string,
        statusCode?: number
    ) {
        super(isSuccessful, message, fileContents, ApiResponseType.File, errorCode, statusCode);
        this.contentType = contentType;
        this.fileDownloadName = fileDownloadName;
        this.fileName = fileName;

        if (isSuccessful) {
            this.addInfo('ContentType', contentType);
            this.addInfo('FileDownloadName', fileDownloadName);
            this.addInfo('FileName', fileName);
        }
    }

    // Default constructor for when no data is available
    static createEmpty(): ApiFileResponse {
        return new ApiFileResponse(true, 'No data available');
    }
}

export class ApiFileResponses {
    static success(
        fileContents: Uint8Array,
        contentType: string,
        fileDownloadName: string,
        fileName: string,
        message: string = 'File retrieved successfully',
        statusCode?: number
    ): ApiFileResponse {
        return new ApiFileResponse(
            true,
            message,
            fileContents,
            contentType,
            fileDownloadName,
            fileName,
            undefined,
            statusCode
        );
    }

    static failure(
        errorMessage: string,
        errorCode?: string,
        statusCode?: number
    ): ApiFileResponse {
        return new ApiFileResponse(
            false,
            errorMessage,
            new Uint8Array(0),
            '',
            '',
            '',
            errorCode,
            statusCode
        );
    }
}
