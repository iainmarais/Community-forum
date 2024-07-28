<script lang="ts" setup>
import type { PropType } from 'vue';
</script>

<template>
    <div class="polaroid-frame">
        <img src="@/assets/media/png/placeholderImage300x300.png" class="polaroid-image" alt="placeholder for image" />
        <div class="polaroid-caption">
            <p class="card-label font-weight-bolder text-dark075 font-size-h5">No gallery items found.</p>
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