<script lang = "ts" setup>
import { useRoute, useRouter } from 'vue-router';
import { onMounted, ref, watch } from 'vue';
import type { TopicFullInfo } from '@/Dto/app/TopicInfo';
import { useToast } from 'vue-toastification';
import { useTopicsStore } from '@/stores/Topics/TopicsStore';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import CreateThreadModal from '@/components/modals/CreateThreadModal.vue';
import { useAppContextStore } from '@/stores/AppContextStore';
import ThreadItem from '@/components/views/Forum/Discussion/ThreadItem.vue';

const topicsStore = useTopicsStore();
const appContextStore = useAppContextStore();

const route = useRoute();
const toast = useToast();
const router = useRouter();
const filterDebounceTimer = ref<NodeJS.Timeout|null>();

const topicId = ref<string>(route.params.topicId as string || "");	

const goBack = () => {
    router.go(-1);
}

const createNewThread = () => {
    $("#createThreadModal").modal("show");
}

const searchTopicsInputPressed = () => {
    if(topicsStore.loading_getTopics && !filterDebounceTimer.value) {
        topicsStore.currentPageNumber = 1;
        topicsStore.getThreadsForTopic();
    }
}

onMounted(() => {
    if (route.params && topicId.value) {
        topicsStore.getTopicFullInfo(topicId.value).then(() => {
            topicsStore.currentPageNumber = 1;
            topicsStore.getThreadsForTopic();
        });
    }
});

watch(() => topicsStore.searchQuery, (newSearchQuery) => {
    if(filterDebounceTimer.value) {
        clearTimeout(filterDebounceTimer.value);
    }
    filterDebounceTimer.value = setTimeout(() => {
        topicsStore.loading_getTopics = true;
        topicsStore.currentPageNumber = 1;
        topicsStore.getThreadsForTopic();
    }, 500);
});

watch(() => route.params.topicId, (newTopicId) => {
    topicId.value = newTopicId as string || "";
    if(topicId.value) {
        topicsStore.getTopicFullInfo(topicId.value);
    }
    topicsStore.currentPageNumber = 1;
    topicsStore.getThreadsForTopic();
});

watch(() => appContextStore.loggedInUser, newValue => {
    if (!newValue) {
        toast.error("You must be logged in to view this page");
        router.push({name: "login"});
    }
});

</script>

<template>
    <div v-if="!topicsStore.loading_getTopicFullInfo && topicsStore.topic" class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Topic: {{ topicsStore.topic.topic.topicName ?? "" }}</span>
            </h3>
            <div class="card-toolbar">
                <input class="form-control form-control-solid w-300px" type="text" placeholder="Search posts" v-model="topicsStore.searchQuery" @keyup.enter="searchTopicsInputPressed"/>
                <button class="btn btn-primary btn-sm m-1" @click="createNewThread"><i class="fas fa-plus"></i>Create new</button>
                <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-arrow-left"></i>Back</button>
            </div>
        </div>
        <div class="card-body">
            <ThreadItem v-if="topicsStore.threads" v-for ="thread in topicsStore.threads.rows" :key="thread.thread.threadId" :thread="thread" />
            <span v-else>No threads have been created for this topic.</span>
        </div>
    </div>
    <LoadingIndicator v-else :loading="topicsStore.loading_getTopicFullInfo" />
    <CreateThreadModal v-if="topicsStore.topic !== undefined" :topicId = topicsStore.topic.topic.topicId />
</template>