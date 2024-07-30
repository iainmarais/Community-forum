<script lang="ts" setup>
import { computed, onMounted, onUnmounted, reactive, ref } from 'vue';
import frame0 from "@/assets/media/png/loadingPlaceholderImageFrame0.png";
import frame1 from "@/assets/media/png/loadingPlaceholderImageFrame1.png";
import frame2 from "@/assets/media/png/loadingPlaceholderImageFrame2.png";
import frame3 from "@/assets/media/png/loadingPlaceholderImageFrame3.png";
const props = defineProps({
    loading: {
        type: Boolean,
        default: true
    },
});

const loadingImages = [
    frame0,
    frame1,
    frame2,
    frame3
];

const reactiveImages = reactive(loadingImages);

const currentIndex = ref(0);

let interval: any;

const updateCurrentIndex = () => {
    currentIndex.value = (currentIndex.value + 1) % loadingImages.length;
};

onMounted(() => {
    interval = setInterval(updateCurrentIndex, 500);
});

onUnmounted(() => {
    clearInterval(interval);
});

</script>

<template>
    <div class="polaroid-frame">
        <img v-show="!loading" src="@/assets/media/png/placeholderImage300x300.png" class="polaroid-image" alt="placeholder for image" />
        <div v-show="loading">
            <img :src="reactiveImages[currentIndex]" class="polaroid-image" alt="Loading..." />
        </div>
        <div class="polaroid-caption">
            <span class="font-weight-bolder"> {{loading ? "Loading..." : ""}} </span>
        </div>
    </div>
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