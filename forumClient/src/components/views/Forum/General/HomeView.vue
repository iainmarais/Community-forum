<script lang = "ts" setup>
import ForumStatsView from '@/components/views/Forum/General/ForumStatsView.vue';
import ThreadListSummaryView from '@/components/views/Forum/Discussion/ThreadListSummaryView.vue';
import TopicListView from '@/components/views/Forum/Topics/TopicListView.vue';
import CategoryView from '@/components/views/Forum/Categories/CategoryView.vue';
import { onMounted, ref, watch } from 'vue';

onMounted(() => {
    
});
</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Forum - Home</span>
            </h3>
        </div>
        <div class="card-body">
            <CategoryView />
            <!--Make this scrollable. Future design idea: Add chat to the right of it so that while looking at the discussion view, users can also chat-->
            <ThreadListSummaryView />
            <ForumStatsView />
        </div> 
    </div>
</template>