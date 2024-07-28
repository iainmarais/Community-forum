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
    <div class="polaroid-frame-md">
        <div class="polaroid-image-background-md">
            <img v-if="imageData && imageData?.length > 0" :src="props.imageData" class="polaroid-image" alt="Image"/>
            <img v-else src="@/assets/media/png/placeholderImage300x300.png" class="polaroid-image" alt="placeholder for image" />
        </div>
        <div class="polaroid-caption">
            <table class="table table-borderless table-sm">
                <tr>
                    <td><span class="font-weight-bolder">Name:</span></td>
                    <td><span>{{ props.imageDescription }}</span></td>
                </tr>
                <tr>
                    <td><span class="font-weight-bolder">Type:</span> </td>
                    <td><span>{{ getFriendlyContentType() }}</span></td>
                </tr>
            </table>
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
  height: 400px !important;
  text-align: center;
  position: relative;
}

.polaroid-frame-md {
  display: flexbox;
  background: white;
  padding: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
  border-radius: 5px;
  width: 400px !important; /* Adjust as needed */
  height: 500px !important;
  text-align: center;
  position: relative;
}

.polaroid-frame-lg {
  display: flexbox;
  background: white;
  padding: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
  border-radius: 5px;
  width: 600px !important; /* Adjust as needed */
  height: 700px !important;
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

.polaroid-image-background-md {
  max-width: 100%;
  height: 380px;
  background: #111;
  display: block;
  margin-bottom: 10px; /* Space between image and caption */
}

.polaroid-image-background-lg {
  max-width: 100%;
  height: 580px;
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

.polaroid-image-md {    
  max-width: 100%;
  height: 380px;
  display: initial;
}

.polaroid-image-lg {    
  max-width: 100%;
  height: 580px;
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

.polaroid-caption-md {
  font-family: Arial, sans-serif;
  max-height: 100%;
  font-size: 14px;
  color: #333;
  padding: 5px;
  height: auto;
  border-top: 1px solid #ddd;
  background-color: #f9f9f9;
}

.polaroid-caption-lg {
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