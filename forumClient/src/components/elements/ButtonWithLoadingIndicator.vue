<script lang = "ts" setup>
    import { defineProps, defineEmits } from 'vue';
    const props = defineProps({
        label: {
            type: String,
        },
        loading: {
            type: Boolean,
            default: false
        },
        disabled: {
            type: Boolean,
            default: false
        },
        buttonClass: {
            type: String,
            default: "btn-primary btn-sm"
        },
        icon: {
            type: String,
            default: ""
        }
    });

    const emit = defineEmits(['click']);

    function handleClick(event: Event) {
        if (!props.loading) {
            emit('click', event);
        }
    }
</script>

<template>
    <button class= "btn btn-primary btn-sm" @click="handleClick">
        <template v-if="loading">
            <v-progress-circular indeterminate color="white" size="20" class="me-2" />
            Loading...
        </template>
        <template v-else>
            <i v-if="icon" :class="icon"></i> {{ label }}
        </template>
    </button>
</template>

<style scoped>
/* 
Optional: Style spinner to align properly with the button content 
*/
.v-progress-circular {
    vertical-align: middle;
}
</style>