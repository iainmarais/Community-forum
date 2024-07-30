<script lang="ts" setup>
import { useBoardsStore } from '@/stores/Boards/BoardsStore';
import { onMounted, ref, watch } from 'vue';
import CreateNewBoardModal from '@/components/modals/CreateNewBoardModal.vue';
import BoardItem from './BoardItem.vue';
import type { BoardBasicInfo } from '@/Dto/app/BoardInfo';

const boardsStore = useBoardsStore();
const boardName = ref<string>("");
const boards = ref<BoardBasicInfo[]>();
const boardVisibility = ref<Record<string, boolean>>({}); // Map of category visibility

onMounted(() => {
    boardsStore.getBoards();
});

watch(() => boardsStore.boards, (newBoards) => {
    if (newBoards.length === 0) return;
    boards.value = newBoards;
    newBoards.forEach((board) => {
        boardVisibility.value[board.boardId] = false; // Initialize visibility state
    });
  }
);

</script>

<template>
    <div class="card card-custom">
      <div class="card-body" v-if="boardsStore.boards?.length > 0">
          <table class="table table-borderless table-sm">
              <BoardItem :board="board" v-for="board in boards" :key="board.boardId" />
          </table>
      </div>
      <div v-else class="card-body">
        <p>No boards found</p>
      </div>
    </div>
    <CreateNewBoardModal />
  </template>