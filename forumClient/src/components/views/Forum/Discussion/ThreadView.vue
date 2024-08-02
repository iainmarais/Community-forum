<script lang="ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { ThreadBasicInfo, ThreadFullInfo } from '@/Dto/app/ThreadInfo';
import UserService from '@/services/UserService';
import { onMounted, ref, watch, type PropType } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
const createdByUser = ref<UserBasicInfo>();
import DateUtils from '@/components/utils/DateUtils';
import { useDiscussionStore } from '@/stores/Discussions/DiscussionStore';

const discussionStore = useDiscussionStore();

const router = useRouter();
const toast = useToast();
const route = useRoute();
const props = defineProps ({
    thread: Object as PropType<ThreadFullInfo>,
});

const threadId = ref<string>(route.params.threadId as string || "");

const viewThread = (threadId: string) => {
    router.push({ name: "ViewThread", params: { threadId: threadId } })
    .catch((error) => {
        toast.error("Could not load thread.");
        console.log(error);
    });
}

watch(() => route.params.threadId, (newThreadId) => {
    threadId.value = newThreadId as string || "";
    //Redirect the user to the home page in the event of this value being "home"
    if(threadId.value === "home") {
        router.push({name: "home"}); 
    }
});

const goBack = () => {
    router.go(-1);
}

onMounted(() => {
  discussionStore.getThreadFullInfo(props.thread!.thread.threadId);
})
</script>

<template>
    <div class="card card-custom">
        <div class="card-body">
            <div class=row>
                <span class="card-label font-weight-bolder text-dark075">{{ props.thread!.thread.threadName }}
                    <button class="btn btn-outline-primary btn-sm ml-3" @click="viewThread(props.thread!.thread.threadId)">
                    <i class="fas fa-eye"></i>
                    View</button>
                </span>
                
            </div>
            <table class ="table table-striped table-hover table-sm" v-if="thread != null">
                <thead>
                    <tr>
                        <th>Created by</th>
                        <th>Created</th>
                        <th>Replies</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>{{ createdByUser?.username || "" }}</td>
                        <td>{{ DateUtils.formatDate(props.thread!.thread.createdDate)}}</td>
                        <td>{{ props.thread!.thread.numberOfPosts }}</td>
                    </tr>
                </tbody>
            </table>
            <p v-else>
            <span>No messages</span>
            </p>
        </div>
    </div>
</template>
