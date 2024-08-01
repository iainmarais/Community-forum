<script lang="ts" setup>
import { type PropType } from 'vue';
import { useToast } from 'vue-toastification';
import GalleryItemInfoModal from './GalleryItemInfoModal.vue';
import type { GalleryItemBasicInfo } from '@/Dto/app/GalleryItemInfo';
import DateUtils from '../utils/DateUtils';

const toast = useToast();
const props = defineProps({ 
    imageData: String as PropType<string>,
    imageName: String as PropType<string>,
    createdDate: Object as PropType<Date>,
    imageDescription: String as PropType<string>,
    contentType: String as PropType<string>,
    item: Object as PropType<GalleryItemBasicInfo>,
    imageToDisplay: Object as PropType<HTMLImageElement>,

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

const showInfoModal = () => {
    $("#infoModal").modal("show");
}

</script>

<template>
    <div class="polaroid-frame-classic">
        <img v-if="imageData && imageData?.length > 0" :src="props.imageData" class="polaroid-image" alt="Image"/>
        <img v-else-if="imageToDisplay" :src="imageToDisplay.src" class="polaroid-image" alt="Image"/>
        <img v-else src="@/assets/media/png/placeholderImage300x300.png" class="polaroid-image" alt="placeholder for image" />
        <div class="polaroid-caption" @click.prevent="showInfoModal()">
            <p class="polaroid-caption-title">Name: {{ props.imageName}}, Created: {{ DateUtils.formatDateOnly(props.createdDate!) }}</p>
            <p class="polaroid-caption-text"> {{ props.imageDescription }}</p>
        </div>
    </div>
<GalleryItemInfoModal :item="props.item"/>
</template>