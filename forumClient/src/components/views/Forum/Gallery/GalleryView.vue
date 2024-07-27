<script lang = "ts" setup>
import { useRouter } from 'vue-router';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import { useGalleryStore } from '@/stores/Gallery/GalleryStore';
import { onMounted, ref } from 'vue';
import type { GalleryItemBasicInfo } from '@/Dto/app/GalleryItemInfo';
import GalleryItem from './GalleryItem.vue';

const router= useRouter();
const galleryStore = useGalleryStore();

const galleryItems = ref<GalleryItemBasicInfo[]>([]);

const goBack = () => {
    router.go(-1);
}

onMounted(() => {
    galleryStore.getGalleryItems();
});
</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Gallery</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-arrow-left"></i>Back</button>
            </div>
        </div>
        <div class="card-body" v-if="!galleryStore.loading_galleryItems && galleryItems.length > 0">
            <div v-for="item in galleryItems" :key="item.galleryItemId">
                <GalleryItem :item="item" />
            </div>
        </div>
        <div class="card-body" v-else>
            <div class="polaroid-frame">
                <img src="@/assets/media/png/placeholderImage300x300.png" class="polaroid-image" alt="placeholder for image" />
                <div class="polaroid-caption">
                    <p class="card-label font-weight-bolder text-dark075 font-size-h5">No gallery items found.</p>
                </div>
            </div>
        </div>
    </div>
    <LoadingIndicator :loading="galleryStore.loading_galleryItems" />
</template>


<style scoped>
.polaroid-frame {
  display: flexbox;
  background: white;
  padding: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
  border-radius: 5px;
  width: 300px !important; /* Adjust as needed */
  height: 360px !important;
  text-align: center;
  position: relative;
}

.polaroid-image {
  max-width: 100%;
  height: auto;
  display: block;
  margin-bottom: 10px; /* Space between image and caption */
}

.polaroid-caption {
  font-family: Arial, sans-serif;
  max-height: 100%;
  font-size: 14px;
  color: #333;
  padding: 5px;
  height: auto;
  border-top: 1px solid #ddd;
  background-color: #f9f9f9;
}
</style>