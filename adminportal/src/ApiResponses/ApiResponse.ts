export class ApiResponse<D> {
    constructor(
        public isSuccessful: boolean,
        public message: string,
        public data?: D,
        public errorCode?: string
    ) {}
}

export class ApiResponseWithMetadata<D, M> extends ApiResponse<D> {
    constructor(
        isSuccessful: boolean,
        message: string,
        data?: D,
        public metadata?: M,
        errorCode?: string
    ) {
        super(isSuccessful, message, data, errorCode);
    }
}