<script lang="ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { TopicBasicInfo } from '@/Dto/app/TopicInfo';
import { ref, watch, type PropType } from 'vue';
import { useToast } from 'vue-toastification';
import { useRouter } from 'vue-router';
import UserService from '@/services/UserService';


const createdByUser = ref<UserBasicInfo>();
const topicName = ref<string>("");
const props = defineProps({
    topic: Object as PropType<TopicBasicInfo>,
});
const router = useRouter();
const toast = useToast();

const viewTopic = (topicId: string) => {
    toast.info("Viewing topic: " + getTopicName());
    //Observation: I need to hit this func twice to get it to route properly. Don't know why. Same is true for the other items and views that are not routed from the navbar
    router.push({ name: "ViewTopic", params: { topicId: topicId } });
}

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
}

watch(() => props.topic, (newTopic) => {
    if (newTopic) {
        topicName.value = newTopic.topic.topicName;
    }
});

const getTopicName = () => {
    return props.topic?.topic.topicName ?? "";
}
</script>

<template>
    <table class="table table-borderless table-sm forum-container">
        <a v-if="props.topic" href="#" @click.prevent="viewTopic(props.topic?.topic.topicId)">
            <tr class="d-flex forum-element" v-if="topic != null">
                <td class="w-50">
                    <div class="d-flex align-items-center">
                        <div class="symbol symbol-50px me-5">
                            <span class="symbol-label font-size-h4 font-weight-bold">
                                <i class="fas fa-folder" style="font-size: 30px;"></i>
                            </span>
                        </div>                                        
                        <div class="ml-3">
                            <div>
                                <span class="text-dark font-weight-bolder text-hover-primary mb-1 font-size-lg">{{ topic.topic.topicName }}</span>
                            </div>
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block">{{ topic.topic.description }}</span>
                            </div>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="d-flex align-items-center">
                        <div class="ml-3">
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block">Created by: {{ getUserInfo(topic.topic.createdByUserId)?.user.username ?? "" }}</span>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </a>
    </table>
</template>