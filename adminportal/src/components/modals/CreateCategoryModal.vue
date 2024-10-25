<script lang = "ts" setup>
import { onMounted, ref, watch } from 'vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import { useToast } from 'vue-toastification';
import type { CreateCategoryRequest } from '@/Dto/AdminPortal/CreateCategoryRequest';
import { Modal } from 'bootstrap';

const toast = useToast();
const contentManagementStore = useContentManagementStore();

const categoryName = ref<string>("");
const categoryDescription = ref<string>("");

const createCategory = () => {
    if(categoryName.value === "" || categoryDescription.value === ""){
        toast.error("Please fill in all fields");
        return;
    }
    const req: CreateCategoryRequest = {
        categoryName: categoryName.value,
        categoryDescription: categoryDescription.value
    }
    contentManagementStore.createCategory(req);
}

const closeModal = () => {
    var modal = document.getElementById("CreateCategoryModal");
    var modalInstance = Modal.getInstance(modal);
    modalInstance?.hide();
}

watch(() => contentManagementStore.result_createCategory, (newValue) => {
    if(newValue) {
        toast.success("Category created successfully");
        categoryName.value = "";
        categoryDescription.value = "";
        contentManagementStore.getCategories();
        closeModal();
    }
})

</script>

<template>
    <div class = "modal fade stackable" id="CreateCategoryModal" tabindex="-1" role="dialog" aria-labelledby="CreateCategoryModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="CreateCategoryModalLabel">Create Category</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="categoryName">Category Name</label>
                        <input type="text" class="form-control" id="categoryName" v-model="categoryName">
                    </div>
                    <div class="form-group">
                        <label for="categoryDescription">Category Description</label>
                        <textarea class="form-control" id="categoryDescription" v-model="categoryDescription"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger btn-sm" data-dismiss="modal" @click="closeModal()">Close</button>
                    <button type="button" class="btn btn-primary btn-sm font-weight-bold" @click="createCategory()">Create</button>
                </div>
            </div>
        </div>
    </div>
</template>