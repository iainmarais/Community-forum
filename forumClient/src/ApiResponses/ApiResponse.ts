export enum ApiResponseType {
    Success = 'Success',
    Failure = 'Failure',
    Error = 'Error',
    File = 'File'
}

export class ApiResponse<D> {
    public additionalInfo?: Record<string, string>;

    constructor(
        public isSuccessful: boolean,
        public message: string,
        public data?: D,
        public responseType: ApiResponseType = ApiResponseType.Success,
        public errorCode?: string,
        public statusCode?: number,
        additionalInfo?: Record<string, string>
    ) {
        this.additionalInfo = additionalInfo;
    }

    public addInfo(key: string, value: string): ApiResponse<D> {
        if (!this.additionalInfo) {
            this.additionalInfo = {};
        }
        this.additionalInfo[key] = value;
        return this;
    }
}

export class ApiResponseWithMetadata<D, M> extends ApiResponse<D> {
    constructor(
        isSuccessful: boolean,
        message: string,
        data?: D,
        public metadata?: M,
        responseType: ApiResponseType = ApiResponseType.Success,
        errorCode?: string,
        statusCode?: number,
        additionalInfo?: Record<string, string>
    ) {
        super(isSuccessful, message, data, responseType, errorCode, statusCode, additionalInfo);
    }
}