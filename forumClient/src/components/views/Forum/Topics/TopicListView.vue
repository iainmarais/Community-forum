<script lang="ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { TopicBasicInfo } from '@/Dto/app/TopicInfo';
import UserService from '@/services/UserService';
import { useTopicListStore } from '@/stores/TopicListStore';
import { onMounted, ref, watch, computed } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from "vue-toastification";

const topicListStore = useTopicListStore();
const createdByUser = ref<UserBasicInfo>();

const toast = useToast();
const router = useRouter();

const topics = ref<TopicBasicInfo[]|undefined>();
const newestTopics = ref<TopicBasicInfo[]|undefined>();


const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
};

const createNewTopic = () => {
    toast.info("Coming soon...");
}

const viewTopic = (topicId: string) => {
    router.push({name:"ViewTopic", params: {topicId: topicId} })
    .catch((error) => {
        toast.error("Could not load topic.");
        console.log(error);
    });
}

onMounted(async () => {
    topicListStore.getTopics();
    topicListStore.getNewestTopics();
});

watch(() => topicListStore.topics, (newTopics) => {
    if (newTopics.length === 0) return;
    topics.value = newTopics;
});

watch(() => topicListStore.newestTopics, (newTopics) => {
    if (newTopics.length === 0) return;
    newestTopics.value = newTopics;
});

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Topics</span>
            </h3>
            <div class="card-toolbar">
                <button class ="btn btn-primary btn-sm" @click="createNewTopic()"><i class="fas fa-plus"></i>Create new</button>
            </div>
        </div>
        <div class="card-body">
            <table class="table table-sm table-hover table-striped" v-if="topicListStore.topics.length > 0">
                <thead>
                    <tr>
                        <th>Topic</th>
                        <th>Created by</th>
                        <th>Created</th>
                        <th>Threads</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="topic in topicListStore.topics" :key="topic.topicId" @click="viewTopic(topic.topicId)">
                        <td>{{ topic.topicName }}</td>
                        <td>{{ getUserInfo(topic.createdByUserId)?.username ?? "" }}</td>
                        <td>{{ topic.createdDate }}</td>
                    </tr>
                </tbody>
            </table>
            <div v-else>
                <p>No topics found</p>
            </div>
        </div>
    </div>
</template>

<style scoped>
.card-body {
    padding: 1.5rem;
}

.list-group-horizontal .list-group-item {
    border: 1px solid #ddd;
    border-radius: 0.25rem;
    margin-right: 0.5rem;
    padding: 0.5rem 1rem;
    flex: 1 1 auto;
}

.table th, .table td {
    vertical-align: middle;
}
</style>