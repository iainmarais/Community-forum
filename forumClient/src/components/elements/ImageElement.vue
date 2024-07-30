<script lang="ts" setup>
import { type PropType } from 'vue';
import { useToast } from 'vue-toastification';

const toast = useToast();
const props = defineProps({ 
    imageData: String as PropType<string>,
    imageDescription: String as PropType<string>,
    contentType: String as PropType<string>,

});

const getFriendlyContentType = () => {
    if(!props.contentType) {
        return "";
    }

    if(props.contentType.includes("image")) {
        return "Image";
    }

    if(props.contentType.includes("video")) {
        return "Video";
    }

    return "Unknown";
}

</script>

<template>
    <div class="polaroid-frame-classic">
        <img v-if="imageData && imageData?.length > 0" :src="props.imageData" class="polaroid-image" alt="Image"/>
        <img v-else src="@/assets/media/png/placeholderImage300x300.png" class="polaroid-image" alt="placeholder for image" />
        <div class="polaroid-caption">
            <p class="polaroid-caption-title">Description: {{ props.imageDescription }}</p>
            <p class="polaroid-caption-text">Type: {{ getFriendlyContentType() }}</p>
        </div>
    </div>
</template>