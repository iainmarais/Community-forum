<script lang = "ts" setup>
import { useThreadsStore } from '@/stores/ThreadsStore';
import type { CreateThreadRequest, CreateThreadWithPostRequest } from '@/Dto/app/ThreadInfo';
import { useToast } from "vue-toastification";
import { ref, watch } from 'vue';
import { useAppContextStore } from '@/stores/AppContextStore';

const threadListStore = useThreadsStore();
const appContextStore = useAppContextStore();

const props = defineProps<{
    topicId: string
}>();
const threadName = ref<string>("");
const messageContent = ref<string>("");
const selectedTopicId = ref<string>(props.topicId);

const toast = useToast();

const createThread = () => {
    if(appContextStore.loggedInUser == null) {
        toast.error("You must be logged in to create a new discussion");
        return;
    }
    if(threadName.value == "") {
        toast.error("Please enter a thread name");
        return;
    }
    if(messageContent.value == null || messageContent.value.length == 0) {
        const request: CreateThreadRequest = {
            topicId: props.topicId,
            threadName: threadName.value,
            createdByUserId: appContextStore.loggedInUser!.userId
        }
        threadListStore.createThread(request);
    }
    if(messageContent.value != null && messageContent.value.length > 0) {
        const request: CreateThreadWithPostRequest = {
            topicId: props.topicId,
            threadName: threadName.value,
            createdByUserId: appContextStore.loggedInUser!.userId,
            messageContent: messageContent.value
        }
        threadListStore.createThreadWithStartingPost(request);
    }
}

watch(() => props.topicId, (newTopicId) => {
    if (newTopicId) {
        selectedTopicId.value = newTopicId;
    }
});

const closeModal = () => {
    $('#createThreadModal').modal("hide");
}

</script>

<template>
    <div id ="createThreadModal" class="modal fade stackable" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <!--note to self: modal-dialog, not modal dialog-->
        <div class="modal-dialog modal-xxl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Create New Discussion</h5>
                    <button type="button" class="close" @click="closeModal()" aria-label="Close">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button> 
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="threadName">Thread Name</label>
                        <input type="text" class="form-control" id="threadName" v-model="threadName">
                    </div>
                    <div class="form-group">
                        <label for="messageContent">Message Content</label>
                        <textarea class="form-control" rows="3" v-model="messageContent"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger" data-dismiss="modal" @click="closeModal">Close</button>
                    <button type="button" class="btn btn-primary" @click="createThread()">Create</button>
                </div>
            </div>
        </div>
    </div>
</template>