<script lang = "ts" setup>
import type { ChatMessageFullInfo } from '@/Dto/app/ChatMessageInfo';
import type { PropType } from 'vue';

const props = defineProps({
    message: Object as PropType<ChatMessageFullInfo>
})
</script>

<template>
    <div class="card card-custom">
        <div class="card-body">
            <!--Layout should be with a tabbed chat/contact list on the right, the chat on the left, and in the same frame as the chat, the input box.-->
        </div>
    </div>
</template>