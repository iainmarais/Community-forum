<script lang = "ts" setup>
import { useRoute, useRouter } from 'vue-router';
import { onMounted, ref, watch } from 'vue';
import type { TopicFullInfo } from '@/Dto/app/TopicInfo';
import { useToast } from 'vue-toastification';
import { useTopicStore } from '@/stores/Topics/TopicStore';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import CreateThreadModal from '@/components/modals/CreateThreadModal.vue';
import type { UserBasicInfo } from '@/Dto/UserInfo';
import UserService from '@/services/UserService';
import DateUtils from '@/components/utils/DateUtils';

const topicStore = useTopicStore();
const route = useRoute();
const toast = useToast();
const router = useRouter();

const topic = ref<TopicFullInfo>();
const createdByUser = ref<UserBasicInfo>();
const topicId = ref<string>(route.params.topicId as string);

const goBack = () => {
    router.go(-1);
}

const createNewThread = () => {
    $("#createThreadModal").modal("show");
}

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

onMounted(() => {
    topicStore.getTopicFullInfo(topicId.value);
});

watch(() => topicStore.topic, (newTopic) => {
    if (newTopic == null) return;
    topic.value = newTopic;
});

</script>

<template>
    <div v-if="!topicStore.loading_getTopicFullInfo" class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Topic: {{ topic?.topic.topicName }}</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm m-1" @click="createNewThread"><i class="fas fa-plus"></i>Create new</button>
                <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-arrow-left"></i>Back</button>
            </div>
        </div>
        <div class="card-body">
            <table class="table table-borderless table-sm" >
                <tr v-for="thread in topic?.threads" :key="thread.threadId">
                    <td>
                        <div class="d-flex align-items-center">
                            <div class="symbol symbol-50px me-5">
                                <span class="symbol-label bg-light">
                                    <i class="fas fa-folder" style="font-size: 30px"></i>
                                </span>
                            </div>
                            <div class="ml-3">
                                <div>
                                    <a class="text-dark font-weight-bolder text-hover-primary mb-1 font-size-lg" @click="viewThread(thread.threadId)"> {{ thread.threadName }}</a>
                                </div>
                                <div>
                                    <span class="text-muted font-weight-bold text-muted d-block">Created by: {{ getUserInfo(thread.createdByUserId)?.username }}</span>
                                </div>
                            </div>
                            <div class="ml-auto">
                                <div>
                                    <span class="text-muted font-weight-bold text-muted d-block"> Created: {{ DateUtils.formatDate(thread.createdDate) }} </span>
                                </div>
                                <div>
                                    <span class="text-muted font-weight-bold text-muted d-block"> Replies: {{ thread.numberOfPosts }} </span>
                                </div>
                                <div>
                                    <span class="text-muted font-weight-bold text-muted d-block"> Has new posts: {{ getHasNewPosts(thread.threadId) ? "Yes" : "No" }} </span>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <LoadingIndicator :loading="topicStore.loading_getTopicFullInfo" />
    <CreateThreadModal :topicId = topicId />
</template>