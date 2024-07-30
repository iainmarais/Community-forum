<script lang = "ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { BoardBasicInfo } from '@/Dto/app/BoardInfo';
import type { ThreadBasicInfo } from '@/Dto/app/ThreadInfo';
import type { TopicBasicInfo } from '@/Dto/app/TopicInfo';
import type { GalleryItemBasicInfo } from '@/Dto/app/GalleryItemInfo';
import type { PropType } from 'vue';

const props = defineProps({
    //This can be topics, threads, boards or users
    item: Object as PropType<BoardBasicInfo|TopicBasicInfo|ThreadBasicInfo|UserBasicInfo>,
})
</script>

<template>
    <div>
        {{ item }}
    </div>
</template>