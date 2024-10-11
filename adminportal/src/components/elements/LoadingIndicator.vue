<script lang= "ts" setup>
import AppLoader from '@/components/elements/Loaders/AppLoader.vue';
 

defineProps({
    loading: {
        type: Boolean,
        required: true,
    }
});
</script>

<template>
    <AppLoader :loading="loading" />
</template>
