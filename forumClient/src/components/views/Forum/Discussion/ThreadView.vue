<script lang="ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { ThreadBasicInfo, ThreadFullInfo } from '@/Dto/app/ThreadInfo';
import UserService from '@/services/UserService';
import { onMounted, ref, type PropType } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
const createdByUser = ref<UserBasicInfo>();
import DateUtils from '@/components/utils/DateUtils';
import { useDiscussionStore } from '@/stores/Discussions/DiscussionStore';

const discussionStore = useDiscussionStore();

const router = useRouter();
const toast = useToast();

const props = defineProps ({
    thread: Object as PropType<ThreadFullInfo>,
});

const viewThread = (threadId: string) => {
    router.push({ name: "ViewThread", params: { threadId: threadId } })
    .catch((error) => {
        toast.error("Could not load thread.");
        console.log(error);
    });
}


const goBack = () => {
    router.push({name: "TopicView", params : {topicId: props.thread!.thread.topicId}});
}

const getUserInfo = async (userId: string) => {
    if (!createdByUser.value) {
        try {
            const response = await UserService.getUserById(userId);
            createdByUser.value = response.data;
        } catch (error) {
            toast.error("Could not load user.");
            console.log(error);
        }
    }
    return createdByUser.value;
};

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
