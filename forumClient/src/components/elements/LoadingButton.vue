<template>
    <button :class="['btn', buttonClass]"  :disabled="loading || disabled" @click="handleClick">
      <!-- Conditional rendering of loading spinner -->
      <template v-if="loading">
        <!-- You can use Vuetify's spinner or Bootstrap's spinner here -->
        <v-progress-circular indeterminate color="white" size="20" class="me-2" />
        Loading...
      </template>
      <template v-else>
        <slot /> <!-- Content of the button when not loading -->
      </template>
    </button>
  </template>
  
<script lang="ts" setup>
import { defineProps, defineEmits } from 'vue';

const props = defineProps({
    label: {
        type: String,
        default: 'Submit',
    },
    loading: {
        type: Boolean,
        default: false,
    },
    disabled: {
        type: Boolean,
        default: false,
    },
    buttonClass: {
        type: String,
        default: 'btn-primary btn-sm', // default Bootstrap button style
    }
});

const emit = defineEmits(['click']);

function handleClick(event: Event) {
    if (!props.loading) {
        emit('click', event); // Only emit click when not loading
    }
}
</script>
  
<style scoped>
/* 
Optional: Style spinner to align properly with the button content 
*/
.v-progress-circular {
    vertical-align: middle;
}
</style>
  