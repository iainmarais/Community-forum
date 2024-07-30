import type { ApiSuccessResponse } from "@/ApiResponses/ApiSuccessResponse";
import type { BoardBasicInfo, BoardFullInfo, CreateBoardRequest } from "@/Dto/app/BoardInfo";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import AxiosClient from "@/http/AxiosClient";

const getBoards = async (): Promise<ApiSuccessResponse<BoardBasicInfo[]>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/boards`);
}
const getSelectedBoard = async (boardId: string): Promise<ApiSuccessResponse<BoardFullInfo>> => {
    return await AxiosClient.Get(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/boards/${boardId}/fullinfo`);
}

const createBoard = async (request: CreateBoardRequest): Promise<ApiSuccessResponse<BoardBasicInfo[]>> => {
    return await AxiosClient.Post(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/forum/boards/create`, request);
}


export default { 
    getBoards, 
    getSelectedBoard,
    createBoard
}