<script lang = "ts" setup>

const props = defineProps({
    size: String,
    light: {
        type: Boolean,
        default: false
    }
});
   
const link  = (): string => {
    if (props.light) {
        return "/applogo2.png";
    }
    return "/applogo2-dark.png";
}
</script>

<template>
    <div class="text-center">
        <img :src="link()" v-if="props.size=='small'" style="max-height: 60px; margin: auto auto; border-radius: 4px;">
        <img :src="link()" v-else-if="props.size=='medium'"  style="max-height: 100px; margin: auto auto; border-radius: 15px;">
        <img :src="link()" v-else  style="max-height: 150px; margin: auto auto; border-radius: 15px;">
    </div>
</template>