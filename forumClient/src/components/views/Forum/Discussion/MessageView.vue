<script lang = "ts" setup>
import type { PostBasicInfo } from '@/Dto/app/PostInfo';
import type { PropType } from 'vue';

const props = defineProps({
    post: Object as PropType<PostBasicInfo>,
    placeholderMessage: Object as PropType<string|undefined>
});

</script>

<template>
    <div class="card card-custom">
        <div class="card-body">
            <p v-if="post">{{ props.post?.postContent }}</p>
            <p v-else>{{ props.placeholderMessage }}</p>
        </div>
    </div>
</template>