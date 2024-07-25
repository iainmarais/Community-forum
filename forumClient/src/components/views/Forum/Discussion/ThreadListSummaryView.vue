<script lang = "ts" setup>
import { onMounted, ref, watch } from 'vue';
import { useThreadsStore } from '@/stores/ThreadsStore';
import type { CreateThreadRequest, ThreadBasicInfo } from '@/Dto/app/ThreadInfo';
import { useToast } from "vue-toastification";
import { useAppContextStore } from '@/stores/AppContextStore';
import CreateThreadModal from '@/components/modals/CreateThreadModal.vue';
import $ from 'jquery';
import type { UserBasicInfo } from '@/Dto/UserInfo';
import UserService from '@/services/UserService';
import { useRoute, useRouter } from 'vue-router';

const threadListStore = useThreadsStore();
const toast = useToast();
const router = useRouter();

const threads = ref<ThreadBasicInfo[]|undefined>();
const createdByUser = ref<UserBasicInfo>();


const createNewDiscussion = () => {
    openCreateThreadModal();
};

const openCreateThreadModal = () => {
    $('#createThreadModal').modal("show");
}

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
};

const viewThread = (threadId: string) => {
    router.push({ name: "ViewThread", params: { threadId: threadId } })
    .catch((error) => {
        toast.error("Could not load thread.");
        console.log(error);
    });
}

onMounted(() => {
    threadListStore.getThreadSummary();
});

watch(() => threadListStore.threads, (newThreads) => {
    if (newThreads.length === 0) return;
    threads.value = newThreads;
});

</script>

<template>
    <div class="row">
        <div class="col-md-12">
            <div class="card card-custom">
                <div class="card-header d-flex align-items-center justify-content-between border-0 pt-7">
                    <h5 class="card-title mb-0">
                        <span class="card-label font-weight-bolder text-dark075 font-size-h5">Discussion Threads</span>
                    </h5> 
                    <div class="card-toolbar">
                        <button class="btn btn-primary btn-sm" @click="createNewDiscussion()"><i class="fas fa-plus"></i>Create new</button>
                    </div>
                </div>
                <div class="card-body">
                    <table class ="table table-striped table-hover table-sm" v-if="threads != null">
                        <thead>
                            <tr>
                                <th>Thread</th>
                                <th>Creator</th>
                                <th>Replies</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="thread in threads" :key="thread.threadId" @click="viewThread(thread.threadId)">
                                <td>{{ thread.threadName }}</td>
                                <td>{{ getUserInfo(thread.createdByUserId)?.username ?? "" }}</td>
                                <td>{{ thread.numberOfPosts }}</td>
                            </tr>
                        </tbody>
                    </table>
                    <p v-if="threads == null">No threads found</p>
                </div>
            </div>
        </div>
    </div>
    <CreateThreadModal />
</template>