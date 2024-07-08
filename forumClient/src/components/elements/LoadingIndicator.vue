<script lang= "ts" setup>
import PulseLoader from 'vue-spinner/src/PulseLoader.vue';

defineProps({
    loading: {
        type: Boolean,
        required: true,
    }
});
</script>

<template>
    <PulseLoader :loading="loading" />
</template>
