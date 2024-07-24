<script lang="ts" setup>
import { ref, watch } from 'vue';
import type { CategoryEntry, CreateCategoryRequest } from "@/Dto/app/CategoryInfo";
import { useAppContextStore } from "@/stores/AppContextStore";
import { useCategoryStore } from "@/stores/Categories/CategoryStore";
import { useToast } from "vue-toastification";

const appContextStore = useAppContextStore();
const categoryStore = useCategoryStore();
const toast = useToast();

const categoryToCreate = ref<CategoryEntry>({} as CategoryEntry);

const createCategoryButtonTooltip = "Create a new category. Categories with no topics will not be shown by default.";

const categoryName = ref<string>("");
const description = ref<string>("");

const createCategory = () => {
    if(appContextStore.loggedInUser == null) {
        toast.error("You must be logged in to create a new category");
        return;
    }
    if(categoryName.value == "") {
        toast.error("Please enter a category name");
        return;
    }
    if(description.value == "") {
        toast.error("Please enter a category description");
        return;
    }
    const request: CreateCategoryRequest = {
        categoryName: categoryName.value,
        categoryDescription: description.value,
    }
    categoryStore.createCategory(request);
}

watch (() => categoryStore.result_createCategorySuccess, (newValue) => {
    if (newValue) {
        toast.success("Category created successfully");
        closeModal();
        categoryStore.getCategories();
    }
});

const closeModal = () => {
    $('#createCategoryModal').modal("hide");
}
</script>

<template>
    <div id ="createCategoryModal" class="modal fade stackable" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <!--note to self: modal-dialog, not modal dialog-->
        <div class="modal-dialog modal-xxl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Create New Category</h5>
                    <button type="button" class="close" @click="closeModal()" aria-label="Close">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="categoryName">Category Name</label>
                                <input type="text" class="form-control" id="categoryName" v-model="categoryName" />
                            </div>
                            <div class="form-group">
                                <label for="description">Description</label>
                                <textarea class="form-control" id="description" v-model="description"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger font-weight-bold" @click="closeModal">Close</button>
                    <button type="button" class="btn btn-primary font-weight-bold" @click="createCategory()">Create</button>
                </div>
            </div>
        </div>
    </div>
</template>