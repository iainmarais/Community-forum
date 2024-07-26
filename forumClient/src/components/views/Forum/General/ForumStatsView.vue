<script lang = "ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import { useForumStatsStore } from '@/stores/ForumStatsStore';
import { onMounted, ref, watch} from 'vue';

const forumStatsStore = useForumStatsStore();

const totalTopics = ref<number>();
const totalThreads = ref<number>();
const totalPosts = ref<number>();
const totalUsers = ref<number>();
const mostActiveUsers = ref<UserBasicInfo[]>();
const popularTopics = ref<number>();

onMounted(() => {
    forumStatsStore.getForumStats();
})

watch(() => forumStatsStore.forumStats, (newStats) => {
    if (newStats == null) return;
    totalTopics.value = newStats.totalTopics ?? 0;
    totalThreads.value = newStats.totalThreads ?? 0;;
    totalPosts.value = newStats.totalPosts ?? 0;;
    totalUsers.value = newStats.totalUsers ?? 0;;
    mostActiveUsers.value = newStats.mostActiveUsers ?? [];
    popularTopics.value = newStats.popularTopics ?? 0;;
});
</script>

<template>
    <table class="table table-borderless table-sm" >
        <tr>
            <td>
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Forum Stats</span>
            </td>
        </tr>
        <tr>    
            <td>
                <table class="table table-borderless table-sm" v-if="forumStatsStore.forumStats != null">
                    <thead>
                        <tr>
                            <th>Total Topics</th>
                            <th>Total Threads</th>
                            <th>Total Posts</th>
                            <th>Total Users</th>
                            <th>Popular Topics</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>{{ totalTopics }}</td>
                            <td>{{ totalThreads }}</td>
                            <td>{{ totalPosts }}</td>
                            <td>{{ totalUsers }}</td>
                            <td>{{ popularTopics }}</td>
                        </tr>
                    </tbody>
                </table>
                <p v-else>No stats found</p>
            </td>
        </tr>
        <tr>
            <td>
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Most Active Users</span>
            </td>
        </tr>
        <tr>
            <td>
                <table class="table table-borderless table-sm" v-if="forumStatsStore.forumStats.mostActiveUsers && forumStatsStore.forumStats.mostActiveUsers.length > 0">
                    <thead>
                        <tr>
                            <th>User</th>
                            <th>Total Posts</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="user in mostActiveUsers" :key="user.userId">
                            <td>{{ user.username }}</td>
                            <td>{{ user.totalPosts }}</td>
                        </tr>
                    </tbody>
                </table>
                <p v-else>No users found</p>
            </td>
        </tr>
    </table>
</template>