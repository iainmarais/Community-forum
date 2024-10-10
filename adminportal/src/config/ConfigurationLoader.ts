const getConfig = () => {
    return {
        apiV1: {
            baseUrl: import.meta.env.VITE_APIV1_BASEURL
        },
        apiV2: {
            baseUrl: import.meta.env.VITE_APIV2_BASEURL
        },
        envName: import.meta.env.VITE_ENVNAME
    }
}

export default { getConfig };