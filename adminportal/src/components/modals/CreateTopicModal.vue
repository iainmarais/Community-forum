<script lang = "ts" setup>
import type { CreateTopicRequest } from '@/Dto/AdminPortal/CreateTopicRequest';
import { ref, watch } from 'vue';
import { useToast } from 'vue-toastification';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import { Modal } from 'bootstrap';

const contentManagementStore = useContentManagementStore();
const toast = useToast();

const topicName = ref<string>("");
const topicDescription = ref<string>("");
const selectedBoardId = ref<string>("");

const boardOptions = ref<Record<string, string>>({});

const populateBoardOptions = () => {
    var foundBoardOptions: Record<string, string> = {};
    if(contentManagementStore.boards.rows.length > 0) {
        for(var element in contentManagementStore.boards.rows) {
            foundBoardOptions[contentManagementStore.boards.rows[element].board.boardId] = contentManagementStore.boards.rows[element].board.boardName;
        }
    }

    boardOptions.value = foundBoardOptions;
}

const closeModal = () => {
    var modal = document.getElementById("CreateTopicModal");
    var modalInstance = Modal.getInstance(modal);
    modalInstance?.hide();
}

const createTopic = () => {

    if (selectedBoardId.value.length == 0 || selectedBoardId.value == null) {
        toast.error("Board Id can't be null or empty");
        return;
    }
    if(topicName.value.length == 0 || topicName.value == null) {
        toast.error("Topic Name can't be null or empty");
        return;   
    }
    if(topicDescription.value.length == 0 || topicDescription.value == null) {
        toast.error("Topic Description can't be null or empty");
        return;
    }

    const req: CreateTopicRequest = {
        boardId: selectedBoardId.value,
        topicName: topicName.value,
        description: topicDescription.value
    }

    contentManagementStore.createTopic(req);
}

watch(() => contentManagementStore.result_createTopic, (newValue) => {
    if(newValue) {
        toast.success("Topic created successfully");
        topicName.value = "";
        topicDescription.value = "";
        selectedBoardId.value = "";
        contentManagementStore.getTopics();
        closeModal();
    }
});

</script>

<template>
    <div class = "modal fade stackable" id="CreateTopicModal" tabindex="-1" role="dialog" aria-labelledby="CreateTopicModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="CreateTopicModalLabel">Create Topic</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="topicName">Topic Name</label>
                        <input type="text" class="form-control" id="topicName" v-model="topicName">
                    </div>
                    <div class="form-group">
                        <label for="topicDescription">Topic Description</label>
                        <textarea class="form-control" id="topicDescription" v-model="topicDescription"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="selectedBoardId">Board</label>
                        <select class="form-control" v-model="selectedBoardId" style="width: 60%" @click="populateBoardOptions()">
                            <option v-for="(value, key) in boardOptions" :key="key" :value="key">{{value}}</option>
                        </select>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger btn-sm" data-dismiss="modal" @click="closeModal()">Close</button>
                    <button type="button" class="btn btn-primary btn-sm font-weight-bold" @click="createTopic()">Create</button>
                </div>
            </div>
        </div>
    </div>
</template>