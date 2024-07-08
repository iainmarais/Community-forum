<script lang = "ts" setup>
import type { MessageBasicInfo } from '@/Dto/app/MessageInfo';
import type { PropType } from 'vue';

const props = defineProps({
    message: Object as PropType<MessageBasicInfo>,
    placeholderMessage: Object as PropType<string|undefined>
});

</script>

<template>
    <div class="card card-custom">
        <div class="card-body">
            <p v-if="message">{{ props.message?.messageContent }}</p>
            <p v-else>{{ props.placeholderMessage }}</p>
        </div>
    </div>
</template>