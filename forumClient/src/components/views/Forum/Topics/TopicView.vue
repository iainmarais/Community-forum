<script lang = "ts" setup>
import { useRoute, useRouter } from 'vue-router';
import { onMounted, ref, watch } from 'vue';
import type { TopicFullInfo } from '@/Dto/app/TopicInfo';
import { useToast } from 'vue-toastification';
import { useTopicStore } from '@/stores/Topics/TopicStore';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import CreateThreadModal from '@/components/modals/CreateThreadModal.vue';
import { useAppContextStore } from '@/stores/AppContextStore';
import ThreadItem from '@/components/views/Forum/Discussion/ThreadItem.vue';

const topicStore = useTopicStore();
const appContextStore = useAppContextStore();

const route = useRoute();
const toast = useToast();
const router = useRouter();

const topic = ref<TopicFullInfo>();

const topicName = ref<string>("");
const topicId = ref<string>(route.params.topicId as string || "");	

const goBack = () => {
    router.go(-1);
}

const createNewThread = () => {
    $("#createThreadModal").modal("show");
}

onMounted(() => {
    if (topicId.value) {
        topicStore.getTopicFullInfo(topicId.value);
    }
});

watch(() => topicStore.topic, (newTopic) => {
    if (newTopic) {
        topic.value = newTopic;
        topicName.value = newTopic.topic.topicName;
    }
});

watch(() => route.params.topicId, (newTopicId) => {
    topicId.value = newTopicId as string || "";
    if(topicId.value === "home") {
        router.push({name: "home"}); 
    }
});

watch(() => appContextStore.loggedInUser, newValue => {
    if (!newValue) {
        toast.error("You must be logged in to view this page");
        router.push({name: "login"});
    }
});

</script>

<template>
    <div v-if="!topicStore.loading_getTopicFullInfo" class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Topic: {{ topicName ?? "" }}</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm m-1" @click="createNewThread"><i class="fas fa-plus"></i>Create new</button>
                <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-arrow-left"></i>Back</button>
            </div>
        </div>
        <div class="card-body">
            <ThreadItem v-if="topic?.threads" v-for ="thread in topic?.threads" :key="thread.threadId" :thread="thread" />
            <span v-else>No threads have been created for this topic.</span>
        </div>
    </div>
    <LoadingIndicator :loading="topicStore.loading_getTopicFullInfo" />
    <CreateThreadModal v-if="topic !== undefined" :topicId = topic.topic.topicId />
</template>