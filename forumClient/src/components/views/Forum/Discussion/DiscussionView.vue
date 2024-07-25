<script lang = "ts" setup>
import { useRoute, useRouter } from 'vue-router';
import { useAppContextStore } from '@/stores/AppContextStore';
import { useDiscussionStore } from '@/stores/Discussions/DiscussionStore';
import { onMounted, ref, watch, type PropType } from 'vue';
import type { ThreadFullInfo } from '@/Dto/app/ThreadInfo';
import MessageView from './MessageView.vue';
import { useToast } from 'vue-toastification';

const props = defineProps({
    thread: Object as PropType<ThreadFullInfo>,
});

const router = useRouter();
const route = useRoute();
const routeParams = route.params;
const threadId = routeParams.threadId as string;
const thread = ref<ThreadFullInfo>();
const discussionStore = useDiscussionStore();
const toast = useToast();

onMounted(() => {
    discussionStore.getThreadFullInfo(threadId);
});

watch(() => discussionStore.thread, (newThread) => {
    if (newThread == null) return;
    thread.value = newThread;
});

const postReply = () => {
    toast.info("Coming soon...");	
}

const goBack = () => {
    router.push({name: "ViewTopic", params : {topicId: thread.value!.thread.topicId}});
}

</script>

<template>
<div class="card card-custom">
    <div class="card-header border-0 pt-7">
        <h3 class="card-title align-items-start flex-column">
            <span class="card-label font-weight-bolder text-dark075 font-size-h5">Discussion: {{ thread?.thread.threadName }}</span>
        </h3>
        <div class="card-toolbar"> 
            <!--How does one add some padding between the elements of the toolbar?-->
            <button class="btn btn-primary btn-sm m-1" @click="postReply"><i class="fas fa-plus"></i>Post reply</button>
            <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-arrow-left"></i>Back</button>
        </div>
    </div>
    <div class="card-body">
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <MessageView v-if="thread != null && thread!.messages.length > 0" v-for = "message in thread?.messages" :key = "message.messageId" :message = "message" />
                    <span v-else>No Messages</span>
                </div>
            </div>
        </div>
    </div>
</div>
</template>