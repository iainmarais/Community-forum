<script lang = "ts" setup>
import type { CreateGalleryItemRequest, DisplayImageData } from '@/Dto/app/GalleryItemInfo';
import { useGalleryStore } from '@/stores/Gallery/GalleryStore';
import { ref, watch } from 'vue';
import { useToast } from 'vue-toastification';
import ImageElement from '../elements/ImageElement.vue';

const galleryStore = useGalleryStore();
const toast = useToast();
const itemName = ref<string>("");
const itemDescription = ref<string>("");
const selectedFile = ref<File|null>(null);

const imageToDisplay = ref<HTMLImageElement|null>(null);

const onFileChange = (event: Event) => {
    const target = event.target as HTMLInputElement;
    if(target.files && target.files[0]) {
        selectedFile.value = target.files[0];
    }
}

const createGalleryItem = () => {
    if(!itemName.value) {
        toast.error("Please enter the name of the item.");
    }
    if(!itemDescription.value) {
        toast.error("Please enter a description.");
    }
    if(!selectedFile.value) {
        toast.error("Please select an item to upload.");
    }
    const formData = new FormData();
    formData.append("galleryItemName", itemName.value);
    formData.append("galleryItemDescription", itemDescription.value);
    formData.append("file", selectedFile.value!);

    galleryStore.createGalleryItem(formData);
}

const closeModal = () => {
    $("#createGalleryItemModal").modal("hide");
}

watch(() => selectedFile.value, newValue => {
    if(newValue) {
        imageToDisplay.value!.src = URL.createObjectURL(newValue);
    }
});

watch(() => galleryStore.result_createGalleryItemSuccess, newValue => {
    if(newValue) {
        closeModal();
    }
});

</script>

<template>
    <div id="createGalleryItemModal" class="modal fade stackable" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-xxl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Create New Gallery Item</h5>
                    <button type="button" class="close" @click="closeModal()" aria-label="Close">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="itemName">Item Name</label>
                        <input type="text" class="form-control" id="itemName" v-model="itemName">
                    </div>
                    <div class="form-group">
                        <label for="description">Description</label>
                        <textarea class="form-control" id="description" v-model="itemDescription"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="image">Image</label>
                        <input type="file" class="form-control" id="image" @change="onFileChange">
                    </div>
                    <div class="form-group">
                        <!--Need to fix this to display the image-->
                        <ImageElement v-if="imageToDisplay" :image-to-display=imageToDisplay />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-light-primary font-weight-bold" @click="closeModal()">Cancel</button>
                    <button type="button" class="btn btn-primary font-weight-bold" @click="createGalleryItem()">Create</button>
                </div>
            </div>
        </div>
    </div>
</template>