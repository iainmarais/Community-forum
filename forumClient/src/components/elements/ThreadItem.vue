<script lang = "ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { ThreadBasicInfo } from '@/Dto/app/ThreadInfo';
import { useThreadsStore } from '@/stores/ThreadsStore';
import { onMounted, ref, watch, type PropType } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import DateUtils from '@/components/utils/DateUtils';
import UserService from '@/services/UserService';

const toast = useToast();
const threadsStore = useThreadsStore();
const createdByUser = ref<UserBasicInfo>();

const numberOfPosts = ref(0);
const numNewPosts = ref(0); //Since the user's last view. Not currently used.

const router = useRouter();

const props = defineProps({
    thread: Object as PropType<ThreadBasicInfo>,
});

onMounted(() => {
    //Todo: Use the onmounted lifecycle hook to get the number of new posts since last view
});

watch(() => props.thread?.thread.topicId, (newTopicId) => {
    if (newTopicId == null) {
        //If this is not null or undefined, use it to grab the basic info of the associated topic, else return.
        return;
    }
    threadsStore.getAssociatedTopic(newTopicId);
});


//Leaving this here for now. Might be useful in the future.
const goBack = () => {
    router.push({name: "TopicView", params : {topicId: props.thread!.thread.topicId}});
}

const viewThread = (threadId: string) => {
    router.push({ name: "ViewThread", params: { threadId: threadId } })
    .catch((error) => {
        toast.error("Could not load thread.");
        console.log(error);
    });
}

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
}

</script>

<template>
    <div class="table-responsive p-0">
        <table class="table table-sm table-borderless table-vertical-center">
            <tbody>
                <tr class="table-active">
                    <td> 
                        <div class="d-flex align-items-center"> 
                            <div class="symbol symbol-40 flex-shrink-0">
                                <div class="symbol-label">
                                    <i class="fas fa-folder-open "></i> 
                                </div>
                            </div>
                            <div class="ml-3">
                                <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">
                                    <a href="#" @click="viewThread(props.thread!.thread.threadId)">{{ props.thread!.thread.threadName }}</a>
                                </div>
                                <div>
                                    <div class="ml-auto">
                                        <span class="text-muted font-weight-bold text-muted font-size-sm d-block">Started by: {{ getUserInfo(props.thread!.thread.createdByUserId!)?.user.username ?? "N/A" }}</span>
                                        <span class="text-muted font-weight-bold text-muted font-size-sm d-block">Date Started: {{ DateUtils.formatDate(props.thread?.thread.createdDate!) ?? "N/A" }}</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>