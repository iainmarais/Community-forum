<script lang = "ts" setup>
import { useRouter } from 'vue-router';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import { useGalleryStore } from '@/stores/Gallery/GalleryStore';
import { onMounted, ref } from 'vue';
import type { GalleryItemBasicInfo } from '@/Dto/app/GalleryItemInfo';

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
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <p>Coming soon...</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="card-body" v-else>
            <span>No gallery items found.</span>
        </div>   
    </div>
    <LoadingIndicator :loading="galleryStore.loading_galleryItems" />
</template>