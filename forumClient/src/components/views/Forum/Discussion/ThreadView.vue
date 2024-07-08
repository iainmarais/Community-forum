<script lang="ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { ThreadBasicInfo } from '@/Dto/app/ThreadInfo';
import UserService from '@/services/UserService';
import { ref, type PropType } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
const createdByUser = ref<UserBasicInfo>();

const router = useRouter();
const toast = useToast();

const props = defineProps ({
    thread: Object as PropType<ThreadBasicInfo>,
    placeholderThread: Object as PropType<string|undefined>
});

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
};
</script>

<template>
    <div class="card card-custom">
        <div class="card-body">
            <div class=row>
                <span class="card-label font-weight-bolder text-dark075">{{ props.thread!.threadName }}</span>
                <button class="btn btn-primary btn-sm" @click="viewThread(props.thread!.threadId)">View</button>
            </div>
            <table class ="table table-striped table-hover table-sm" v-if="thread != null">
                <thead>
                    <tr>
                        <th>Created by</th>
                        <th>Created</th>
                        <th>Replies</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>{{ getUserInfo(props.thread!.createdByUserId)!.username }}</td>
                        <td>{{ props.thread?.createdDate}}</td>
                        <td>{{ props.thread!.numberOfPosts }}</td>
                    </tr>
                </tbody>
            </table>
            <p v-else>{{ props.placeholderThread }}</p>
        </div>
    </div>
</template>
