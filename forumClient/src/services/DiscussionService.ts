import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { ThreadFullInfo } from "@/Dto/app/ThreadInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getThreadFullInfo = async (threadId: string): Promise<ApiSuccessResponse<ThreadFullInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/thread/${threadId}/fullinfo`);
}

export default {
    getThreadFullInfo
}