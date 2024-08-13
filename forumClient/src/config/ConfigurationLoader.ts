const getConfig = () => {
    return {
        apiV1: {
            baseUrl: import.meta.env.VITE_API_V1_BASE_URL
        },
        signalRV1: {
            baseUrl: import.meta.env.VITE_SIGNALR_V1_BASE_URL
        },
        //Note to self: For the next version of this backend API, when I eventually get around to finishing the first version, lolz.
        apiV2: {
            baseUrl: import.meta.env.VITE_API_V2_BASE_URL
        },
        signalRV2: {
            baseUrl: import.meta.env.VITE_SIGNALR_V2_BASE_URL
        },
        envName: import.meta.env.VITE_ENV_NAME
    }
}

export default { getConfig };