<script lang="ts" setup>
import { type PropType } from 'vue';
import { useToast } from 'vue-toastification';

const toast = useToast();
const props = defineProps({ 
    imageData: Object as PropType<string>,
    imageDescription: Object as PropType<string>,
    contentType: Object as PropType<string>,

});

</script>

<template>
    <div class="polaroid-frame">
        <div class="polaroid-image-background">
            <img id="galleryItemImage" v-if="imageData && imageData?.length > 0" :src="props.imageData" class="polaroid-image" alt="Image"/>
            <img v-else src="@/assets/media/png/placeholderImage300x300.png" class="polaroid-image" alt="placeholder for image" />
        </div>
        <div class="polaroid-caption">
            <p class="card-label font-weight-bolder text-dark075 font-size-h5">{{ props.imageDescription ?? "Nothing to display" }}</p>
        </div>
    </div>
</template>

<style scoped lang="scss">
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

.polaroid-image-background {
  max-width: 100%;
  height: 280px;
  background: #111;
  display: block;
  margin-bottom: 10px; /* Space between image and caption */
}

//Center this image - not sure how though
.polaroid-image {    
  max-width: 100%;
  height: 280px;
  display: initial;
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