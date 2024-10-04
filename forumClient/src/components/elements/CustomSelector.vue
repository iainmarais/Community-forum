<script lang = "ts" setup>
import type { PropType } from 'vue';

const props = defineProps({
    //Create an option array consisting of {value, displayText}
    options: {
        type: Array as PropType<{value: string, displayText: string}[]>,
        required: true
    },
    selectedValue: {
        type: String,
        required: true
    },
    //CSS classes for styling
    styleClass: {
        type: String
    }
});
/*
Future plans:
Expand this to pull data from an API endpoint and dynamically populate the options?
Add a prop to specify such?
*/
const emit = defineEmits(['update:selectedValue']);

const handleChange = (event: Event) => {
    const target = event.target as HTMLInputElement;
    emit('update:selectedValue', target.value);
}

</script>

<template>
    <select :class="styleClass" aria-label="Default select example" v-model="props.selectedValue" @change="handleChange">
        <option v-for="option in props.options" :value="option.value">{{ option.displayText }}</option>
    </select>
</template>