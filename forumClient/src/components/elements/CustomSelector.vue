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