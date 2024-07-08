<script lang = "ts" setup>
import { useRoute, useRouter } from 'vue-router';
import { onMounted, ref, watch } from 'vue';
import type { TopicFullInfo } from '@/Dto/app/TopicInfo';
import ThreadView from '@/components/views/Forum/Discussion/ThreadView.vue';
import { useToast } from 'vue-toastification';
import { useTopicStore } from '@/stores/Topics/TopicStore';

const topicStore = useTopicStore();

const topic = ref<TopicFullInfo>();
const route = useRoute();
const toast = useToast();
const router = useRouter();

const goBack = () => {
    router.go(-1);
}


onMounted(() => {
    topicStore.getTopicFullInfo(route.params.topicId as string);
});

watch(() => topicStore.topic, (newTopic) => {
    if (newTopic == null) return;
    topic.value = newTopic;
});
</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Topic: {{ topic?.topic.topicName }}</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm"><i class="fas fa-plus"></i>Create new</button>
                <button class="btn btn-primary btn-sm" @click="goBack"><i class="fas fa-arrow-left"></i>Back</button>
            </div>
        </div>
        <div class="card-body">
            <ThreadView v-if="topic != null && topic.threads.length > 0" v-for = "thread in topic?.threads" :key = "thread.threadId" :thread = "thread" />
            <ThreadView v-else :placeholder-thread= '"No threads"' />
        </div>
    </div>
</template>