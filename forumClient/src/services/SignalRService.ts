import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { SignalRConnectionResponse } from "@/Dto/SignalRConnectionResponse";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const signalRV1HttpBaseUrl = `${ConfigurationLoader.getConfig().apiV1.baseUrl}/hubs`;

const postNegotiate = async (version: number, hubName: string): Promise<ApiSuccessResponse<SignalRConnectionResponse>> => {
    return await AxiosClient.PostNegotiate<SignalRConnectionResponse>(`${signalRV1HttpBaseUrl}/${hubName}/negotiate?negotiateVersion=${version}`, {});
}


export default {
    postNegotiate
}