<script lang = "ts" setup>
import type { GalleryItemBasicInfo } from '@/Dto/app/GalleryItemInfo';
import type { PropType } from 'vue';

const props = defineProps ({
    item: Object as PropType<GalleryItemBasicInfo>,
});

function openInNewTab(url: string) {
    window.open(url, '_blank');
}

//Load the image from the gallery item link and display it. It is a url pointing to the image.
const loadImage = () => {
    if(!props.item?.galleryItemLink) {
        //If null, return.
        return;
    }
    const imgElement = document.getElementById("galleryItemImage") as HTMLImageElement;
    imgElement.src = props.item?.galleryItemLink;
}

</script>

<template>
    <!-- Gallery Item - should resemble a stylised photo frame based on the appearance of old polaroid images with the information block in the whitespace below the image.-->
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Image: {{ props.item?.galleryItemName ?? "" }}</span>
            </h3>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="polaroid-frame-classic">
                        <img id="galleryItemImage" class="polaroid-image" @load="loadImage" />
                        <div class="polaroid-caption">
                            <span class="card-label font-weight-bolder text-dark075 font-size-h5">Description: {{ props.item?.galleryItemDescription ?? "" }}</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
