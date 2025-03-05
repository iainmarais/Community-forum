import type { BoardBasicInfo, BoardFullInfo, CreateBoardRequest } from "@/Dto/app/BoardInfo";
import { defineStore } from "pinia";
import ErrorHandler from "@/Handlers/ErrorHandler";
import BoardService from "@/services/BoardService";

type BoardsStoreState = {
    boards?: BoardBasicInfo[],
    selectedBoard?: BoardFullInfo,

    loading_getBoards: boolean,
    result_getBoardsSuccess: boolean,

    loading_createBoard: boolean,
    result_createBoardSuccess: boolean,

    loading_getSelectedBoard: boolean,
    result_getSelectedBoardSuccess: boolean,
}

const defaultState: BoardsStoreState = {
    boards: [],
    selectedBoard: {} as BoardFullInfo,

    loading_getBoards: false,
    result_getBoardsSuccess: false,

    loading_createBoard: false,
    result_createBoardSuccess: false,

    loading_getSelectedBoard: false,
    result_getSelectedBoardSuccess: false
}

export const useBoardsStore = defineStore({
    id: "boards",
    state: () => (defaultState),
    getters: {

    },
    actions: {
        getBoards() {
            this.loading_getBoards = true;
            this.result_getBoardsSuccess = false;
            BoardService.getBoards().then((response) => {
                this.boards = response.data;
                this.result_getBoardsSuccess = true;
                this.loading_getBoards = false;
            }, (error) => {
                ErrorHandler.handleApiErrorResponse(error);
                this.loading_getBoards = false;
            });
        },

        getSelectedBoard(boardId: string) {
            this.loading_getSelectedBoard = true;
            this.result_getSelectedBoardSuccess = false;
            BoardService.getSelectedBoard(boardId).then((response) => {
                this.selectedBoard = response.data;
                this.result_getSelectedBoardSuccess = true;
                this.loading_getSelectedBoard = false;
            }, (error) => {
                ErrorHandler.handleApiErrorResponse(error);
                this.loading_getSelectedBoard = false;
            });
        },
        createBoard(request:CreateBoardRequest) {
            this.loading_createBoard = true;
            this.result_createBoardSuccess = false;
            BoardService.createBoard(request).then((response) => {
                this.boards = response.data;
                this.result_createBoardSuccess = true;
                this.loading_createBoard = false;
            }, (error) => {
                ErrorHandler.handleApiErrorResponse(error);
                this.loading_createBoard = false;
            });
        }
    }
})