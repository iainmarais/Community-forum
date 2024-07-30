<script lang = "ts" setup>
import type { BoardFullInfo } from '@/Dto/app/BoardInfo';
import CreateNewTopicModal from '@/components/modals/CreateNewTopicModal.vue';
import { useBoardsStore } from '@/stores/Boards/BoardsStore';
import { onMounted, ref, watch } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import { useAppContextStore } from '@/stores/AppContextStore';
import { useToast } from 'vue-toastification';
import TopicItem from '@/components/views/Forum/Topics/TopicItem.vue';


const router = useRouter();
const board = ref<BoardFullInfo>({} as BoardFullInfo);
const route = useRoute();
const boardId = ref<string>(route.params.boardId as string || "");

const boardsStore = useBoardsStore();
const appContextStore = useAppContextStore();
const boardName = ref<string>("");
const toast = useToast();

onMounted(() => {
    if(boardId.value) {
        boardsStore.getSelectedBoard(boardId.value);        
    }
});

watch(() => route.params.boardId, (newBoardId) => {
    boardId.value = newBoardId as string || "";
    //Redirect the user to the home page in the event of this value being "home"
    if(boardId.value === "home") {
        router.push({name: "home"}); 
    }
});

watch(() => appContextStore.loggedInUser, newValue => {
    if (!newValue) {
        toast.error("You must be logged in to view this page");
        router.push({name: "login"});
    }
});

watch (() => boardsStore.selectedBoard, (newBoard) => {
    if(newBoard) {
        board.value = newBoard;
        boardName.value = newBoard.board.boardName;
    }
})


const goBack = () => {
    router.go(-1);
}

const createTopic = () => {
    $("#createTopicModal").modal("show");
}

const handleTopicCreated = () => {
    boardsStore.getSelectedBoard(boardId.value);
}

</script>

<template>
    <div class="card card-custom" v-if="!boardsStore.loading_getSelectedBoard">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span v-if="board !== undefined" class="card-label font-weight-bolder text-dark075 font-size-h5">Topics for Board: {{ boardName }}</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm m-1" @click="goBack()"><i class="fas fa-arrow-left"></i>Back</button>
                <button class="btn btn-primary btn-sm m-1" @click="createTopic"><i class="fas fa-plus"></i>Create new topic</button>
            </div>
        </div>
        <div class="card-body">
            <TopicItem v-for="topic in board.topics" :topic="topic" />
        </div>
    </div>
    <LoadingIndicator :loading="boardsStore.loading_getSelectedBoard" />
    <CreateNewTopicModal :selectedBoardId="boardId"  @topicCreated="handleTopicCreated"/>
</template>