<script lang="ts" setup>
import type { CreateBoardRequest } from '@/Dto/app/BoardInfo';
import type { CreateTopicRequest } from '@/Dto/app/TopicInfo';
import { useBoardsStore } from '@/stores/Boards/BoardsStore';
import { ref, watch } from 'vue';
import { useToast } from 'vue-toastification';

const props = defineProps ({
    selectedCategoryId: {
        type: String,
        default: "",
        required: true
    }
});

const emit = defineEmits(['boardCreated']);

const toast = useToast();
const boardsStore = useBoardsStore();

const boardName = ref<string>("");
const boardDescription = ref<string>("");

const closeModal = () => {
    $('#createBoardModal').modal("hide");
}


watch(() => boardsStore.result_createBoardSuccess, (newValue) => {
    if (newValue) {
        toast.success("Board created successfully");
        emit('boardCreated');
        closeModal();
    }
})

const createBoard = () => {
    if(boardName.value == "") {
        toast.error("Please enter a board name");
        return;
    }
    if(boardDescription.value == "") {
        toast.error("Please enter a board description");
        return;
    }
    if(props.selectedCategoryId.length == 0) {
        toast.error("Category id can't be empty");
        return;
    }
    const request: CreateBoardRequest = {
        boardName: boardName.value,
        boardDescription: boardDescription.value,
        //The category id should be the one that the topics in this category share.
        categoryId: props.selectedCategoryId,
    }
    boardsStore.createBoard(request);
}

</script>

<template>
    <div id = "createBoardModal" class = "modal fade stackable" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-xxl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Create New Board</h5>
                    <button type="button" class="close" @click="closeModal()" aria-label="Close">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="topicName">Board Name</label>
                        <input type="text" class="form-control" id="topicName" v-model="boardName">
                    </div>
                    <div class="form-group">
                        <label for="topicDescription">Board Description</label>
                        <textarea class="form-control" id="topicDescription" v-model="boardDescription"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger font-weight-bold" @click="closeModal()">Cancel</button>
                    <button type="button" class="btn btn-primary font-weight-bold" @click="createBoard()">Create</button>
                </div>
            </div>
        </div>
    </div>
</template>