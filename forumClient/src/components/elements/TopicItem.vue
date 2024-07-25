<script lang = "ts" setup>
import type { TopicBasicInfo, TopicFullInfo } from '@/Dto/app/TopicInfo';
import { onMounted, ref, type PropType } from 'vue';
import dayjs from 'dayjs';
import type { UserBasicInfo } from '@/Dto/UserInfo';
import UserService from '@/services/UserService';
import { useRouter } from 'vue-router';
import DateUtils from '@/components/utils/DateUtils';

const createdByUser = ref<UserBasicInfo>();
const router = useRouter();

const numTotalThreads = ref(0);
const numNewThreads = ref(0);

const props = defineProps({
    topic: {
        type: Object as PropType<TopicBasicInfo>,
        required: true
    }
});

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
}

const ViewTopic = (topicId: string) => {
    //Todo: Go to the view topic page using the router with the topic id as a parameter.
    router.push({ name: "ViewTopic", params: { topicId: topicId }});
}

onMounted(() => {
    //Todo: Use the onmounted lifecycle hook to get the number of new threads since the last view
});

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
                                    <!--Replace this with a condition check for any new posts since last view. This might involve a bit of backend work on the topic datamodel to set a last seen date.-->
                                    <i class="fas fa-folder-open "></i> 
                                </div>
                            </div>
                            <div class="ml-3">
                                <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">
                                    <a href="#" @click="ViewTopic(topic.topicId)">{{ topic.topicName }}</a>
                                </div>
                                <div>
                                    <span class="text-muted font-weight-bold text-muted d-block">{{ topic.description }}</span>
                                </div>
                            </div>
                            <div class="ml-auto">
                                <span class="text-muted font-weight-bold text-muted d-block">Total threads: {{ topic.numTotalThreads }}</span>
                                <span class="text-muted font-weight-bold text-muted d-block">New threads: {{ topic.numNewThreads }} </span>
                            </div>
                            <div class="ml-auto">
                                <span class="text-muted font-weight-bold text-muted d-block">{{ `Created by: ${getUserInfo(topic.createdByUserId)?.username}`?? "" }}</span>
                                <span class="text-muted font-weight-bold text-muted d-block">{{ DateUtils.formatDate(topic.createdDate) }}</span>
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div> 
</template>