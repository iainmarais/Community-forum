
<script lang = "ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { ThreadBasicInfo } from '@/Dto/app/ThreadInfo';
import router from '@/router';
import UserService from '@/services/UserService';
import { useToast } from 'vue-toastification';
import { ref, type PropType } from 'vue';
import DateUtils from '@/components/utils/DateUtils';
import { useTopicStore } from '@/stores/Topics/TopicStore';

const topicStore = useTopicStore();
const toast = useToast();
const props = defineProps({
    thread: Object as PropType<ThreadBasicInfo>
});

const createdByUser = ref<UserBasicInfo>();
const viewThread = (threadId: string) => {
    toast.info("Viewing thread: " + getThreadInfo(threadId)?.threadName);
    router.push({ name: "ViewThread", params: { threadId: threadId } });
}

const getHasNewPosts = (threadId: string) => {
    var thread = topicStore.topic?.threads?.find((thread) => thread.threadId === threadId);
    return thread?.hasNewPosts ?? false;
}

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
}

const getThreadInfo = (threadId: string) => {
    var thread = topicStore.topic?.threads?.find((thread) => thread.threadId === threadId);
    return thread;
}
</script>

<template>
    <table class="table table-borderless table-sm forum-container" v-if="props.thread != null">
        <a href="#" @click.prevent=viewThread(props.thread.threadId)>
            <tr class="d-flex forum-element">
                <td class="forum-element-mid-datablock">
                    <div class="d-flex align-items-center">
                        <div class="symbol symbol-50px me-5">
                            <span class="symbol-label bg-light">
                                <i class="fas fa-folder" style="font-size: 30px"></i>
                            </span>
                        </div>
                        <div class="ml-3">
                            <div>
                                <a class="text-dark font-weight-bolder text-hover-primary mb-1 font-size-lg" @click="viewThread(props.thread.threadId)"> {{ props.thread.threadName }}</a>
                            </div>
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block">Created by: {{ getUserInfo(props.thread.createdByUserId)?.username }}</span>
                            </div>
                        </div>
                    </div>
                </td>
                <td class="forum-element-small-datablock">
                    <div class="d-flex align-items-center">
                        <div class="ml-auto">
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block"> Created: {{ DateUtils.formatDate(props.thread.createdDate) }} </span>
                            </div>
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block"> Replies: {{ props.thread.numberOfPosts }} </span>
                            </div>
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block"> Has new posts: {{ getHasNewPosts(props.thread.threadId) ? "Yes" : "No" }} </span>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </a>
    </table>
</template>