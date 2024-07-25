<script lang = "ts" setup>
import { useThreadsStore } from '@/stores/ThreadsStore';
import { useAppContextStore } from '@/stores/AppContextStore';

import type { CreateThreadRequest, CreateThreadWithPostRequest } from '@/Dto/app/ThreadInfo';
import { useToast } from "vue-toastification";
import { onMounted, ref } from 'vue';

const threadListStore = useThreadsStore();
const appContextStore = useAppContextStore();

const topicId = ref<string>("");
const threadName = ref<string>("");
const messageContent = ref<string>("");
const toast = useToast();

const createThread = () => {
    if(appContextStore.loggedInUser == null) {
        toast.error("You must be logged in to create a new discussion");
        return;
    }
    if(topicId.value == "") {
        toast.error("Please select a topic");
        return;
    }
    if(threadName.value == "") {
        toast.error("Please enter a thread name");
        return;
    }
    if(messageContent.value == null || messageContent.value.length == 0) {
        const request: CreateThreadRequest = {
            topicId: topicId.value,
            threadName: threadName.value,
            createdByUserId: appContextStore.loggedInUser!.userId
        }
        threadListStore.createThread(request);
    }
    if(messageContent.value != null && messageContent.value.length > 0) {
        const request: CreateThreadWithPostRequest = {
            topicId: topicId.value,
            threadName: threadName.value,
            createdByUserId: appContextStore.loggedInUser!.userId,
            messageContent: messageContent.value
        }
        threadListStore.createThreadWithStartingPost(request);
    }
}

const closeModal = () => {
    $('#createThreadModal').modal("hide");
}

onMounted(() => {
    appContextStore.getTopics();
});

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
                        <label for="topicId">Topic</label>
                        <select id="topicId" class="form-control" v-model="topicId">
                            <option value="" disabled selected>Select topic</option>
                            <option v-for="topic in appContextStore.topics" :key="topic.topicId" :value="topic.topicId">{{ topic.topicName }}</option>
                        </select>
                    </div>
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