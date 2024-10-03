<script lang ="ts" setup>
import { useRouter, useRoute } from 'vue-router';
import { useBoardsStore } from '@/stores/Boards/BoardsStore';
import { useToast } from 'vue-toastification';
import { onMounted, ref, watch, type PropType } from 'vue';
import type { BoardBasicInfo } from '@/Dto/app/BoardInfo';
import type { UserBasicInfo } from '@/Dto/UserInfo';
import UserService from '@/services/UserService';

const createdByUser = ref<UserBasicInfo>();
const boardName = ref<string>("");
const props = defineProps({
    board: Object as PropType<BoardBasicInfo>
});

const toast = useToast();
const router = useRouter();

const viewBoard = (boardId: string) => {
    toast.info("Viewing board: " + getBoardName());
    router.push({ name: "ViewBoard", params: { boardId: boardId } });
}

const getBoardName = () => {
    return props.board?.board.boardName ?? "";
}

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
}

watch(() => props.board, (newBoard) => {
    if (newBoard) {
        boardName.value = newBoard.board.boardName;
    }
});

</script>

<template> 
    <table class="table table-borderless table-sm forum-container">
        <tbody v-if="props.board" href="#" @click.prevent="viewBoard(props.board.board.boardId)">
            <tr class="d-flex forum-element" v-if="board != null">
                <td class="w-50">
                    <div class="d-flex align-items-center">
                        <div class="symbol symbol-50px me-5">
                            <span class="symbol-label font-size-h4 font-weight-bold">
                                <i class="fas fa-folder" style="font-size: 30px;"></i>
                            </span>
                        </div>                                        
                        <div class="ml-3">
                            <div>
                                <span class="text-dark font-weight-bolder text-hover-primary mb-1 font-size-lg">{{ board.board.boardName }}</span>
                            </div>
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block">{{ board.board.boardDescription }}</span>
                            </div>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="d-flex align-items-center">
                        <div class="ml-3">
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block">Created by: {{ getUserInfo(board.board.createdByUserId)?.user.username ?? "" }}</span>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</template>