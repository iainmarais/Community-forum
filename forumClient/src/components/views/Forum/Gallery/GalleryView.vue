<script lang = "ts" setup>
import { useRouter } from 'vue-router';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import { useGalleryStore } from '@/stores/Gallery/GalleryStore';
import { onMounted, ref, watch } from 'vue';
import type { GalleryItemBasicInfo } from '@/Dto/app/GalleryItemInfo';
import PlaceholderItem from './PlaceholderItem.vue';
import CreateGalleryItemModal from '@/components/modals/CreateGalleryItemModal.vue';
import ImageElement from '@/components/elements/ImageElement.vue';
import { useToast } from 'vue-toastification';

//Future plans for the gallery:
// 1. Add pagination - mission critical for the gallery
// 2. Add search - important, especially when it grows larger.
// 3. Add suport for other media files such as video and audio.


const toast = useToast();
const router= useRouter();
const galleryStore = useGalleryStore();

const galleryItems = ref<GalleryItemBasicInfo[]>({} as GalleryItemBasicInfo[]);

const goBack = () => {
    router.push({name: "home"});
}

const createGalleryItem = () => {
    $("#createGalleryItemModal").modal("show");
}

onMounted(() => {
    galleryStore.getGalleryItems();
});

watch(() => galleryStore.galleryItems, (newItems) => {
    if(newItems) {
        galleryItems.value = newItems;
    }
})
</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Gallery</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm m-1" @click="createGalleryItem"><i class="fas fa-plus"></i>Create new</button>
                <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-arrow-left"></i>Back</button>
            </div>
        </div>
        <div class="card-body" v-if="!galleryStore.loading_galleryItems && galleryItems">
            <div class="row">
                <div class="imageContainer" v-for="item in galleryItems" :key="item.galleryItemId">
                    <ImageElement :image-description="item.galleryItemDescription" :image-data="`data:${item.imageData.contentType};base64,${item.imageData.fileContents}`" :content-type="item.imageData.contentType" />
                </div>
            </div>
        </div>
        <div class="card-body" v-else-if="galleryStore.loading_galleryItems">
            <div class="row">
                <div class="imageContainer">
                    <PlaceholderItem :loading="galleryStore.loading_galleryItems" />
                </div>
            </div>
        </div>
        <div class="card-body" v-else>
            <div class="row">
                <div class="imageContainer">
                    <PlaceholderItem :loading="false" />
                </div>
            </div>
        </div>        
    </div>
    <LoadingIndicator :loading="galleryStore.loading_galleryItems" />
    <CreateGalleryItemModal />
</template>

<style>
    .imageContainer {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 30px;
    }
</style>