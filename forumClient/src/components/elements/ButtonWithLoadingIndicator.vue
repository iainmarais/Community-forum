<script lang = "ts" setup>
    const props = defineProps({
        loading: {
            type: Boolean,
            default: false
        },
        disabled: {
            type: Boolean,
        },
        ButtonClass: {
            type: String
        },
        LoadingClass: {
            type: String,
            default: "spinner-border-sm spinner-right"
        }
    });

    const GetLoadingClass = () => {
        var buttonClass= `${props.ButtonClass || ''}`
        if(props.loading) {
            buttonClass += props.LoadingClass;
        }
        return buttonClass;
    }
</script>

<template>
    <button :disabled="disabled || props.loading" :class="GetLoadingClass()">
        <slot></slot>
    </button>
</template>