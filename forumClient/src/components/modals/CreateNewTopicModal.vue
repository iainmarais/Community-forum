<script lang="ts" setup>
import type { CreateTopicRequest } from '@/Dto/app/TopicInfo';
import { useTopicsStore } from '@/stores/Topics/TopicsStore';
import { ref, watch } from 'vue';
import { useToast } from 'vue-toastification';

const props = defineProps ({
    selectedBoardId: {
        type: String,
        default: "",
        required: true
    }
});

const emit = defineEmits(['topicCreated']);

const toast = useToast();
const topicListStore = useTopicsStore();

const topicName = ref<string>("");
const topicDescription = ref<string>("");

const closeModal = () => {
    $('#createTopicModal').modal("hide");
}


watch(() => topicListStore.result_createNewTopicSuccess, (newValue) => {
    if (newValue) {
        toast.success("Topic created successfully");
        emit('topicCreated');
        closeModal();
    }
})

const createTopic = () => {
    if(topicName.value == "") {
        toast.error("Please enter a topic name");
        return;
    }
    if(topicDescription.value == "") {
        toast.error("Please enter a topic description");
        return;
    }
    if(props.selectedBoardId.length == 0) {
        toast.error("Category id can't be empty");
        return;
    }
    const request: CreateTopicRequest = {
        topicName: topicName.value,
        description: topicDescription.value,
        //The category id should be the one that the topics in this category share.
        boardId: props.selectedBoardId,
    }
    topicListStore.createNewTopic(request);
}

</script>

<template>
    <div id = "createTopicModal" class = "modal fade stackable" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-xxl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Create New Topic</h5>
                    <button type="button" class="close" @click="closeModal()" aria-label="Close">
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
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger font-weight-bold" @click="closeModal()">Cancel</button>
                    <button type="button" class="btn btn-primary font-weight-bold" @click="createTopic()">Create</button>
                </div>
            </div>
        </div>
    </div>
</template>