<script lang = "ts" setup>
import { onMounted, ref, watch } from 'vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import { useToast } from 'vue-toastification';
import { Modal } from 'bootstrap';
import type { CreateBoardRequest } from '@/Dto/AdminPortal/CreateBoardRequest';

const toast = useToast();
const contentManagementStore = useContentManagementStore();

const categoryOptions = ref<Record<string, string>>({});

const boardName = ref<string>("");
const boardDescription = ref<string>("");
const selectedCategoryId = ref<string>("");

const createBoard = () => {
    if(boardName.value === "" || boardDescription.value === ""){
        toast.error("Please fill in all fields");
        return;
    }
    const req: CreateBoardRequest = {
        boardName: boardName.value,
        description: boardDescription.value,
        categoryId: selectedCategoryId.value
    }
    contentManagementStore.createBoard(req);
}

const populateCategoryOptions = () => {
    var foundCategoryOptions: Record<string, string> = {};
    if(contentManagementStore.categories.rows.length > 0) {
        for(var element in contentManagementStore.categories.rows) {
            foundCategoryOptions[contentManagementStore.categories.rows[element].category.categoryId] = contentManagementStore.categories.rows[element].category.categoryName;
        }
    }

    categoryOptions.value = foundCategoryOptions;
}

const closeModal = () => {
    var modal = document.getElementById("CreateBoardModal");
    var modalInstance = Modal.getInstance(modal);
    modalInstance?.hide();
}

watch(() => contentManagementStore.result_createBoard, (newValue) => {
    if(newValue) {
        toast.success("Category created successfully");
        boardName.value = "";
        boardDescription.value = "";
        selectedCategoryId.value = "";
        contentManagementStore.getBoards();
        closeModal();
    }
})

</script>

<template>
    <div class = "modal fade stackable" id="CreateBoardModal" tabindex="-1" role="dialog" aria-labelledby="CreateBoardModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="CreateBoardModalLabel">Create Board</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="categoryName">Board Name</label>
                        <input type="text" class="form-control" id="categoryName" v-model="boardName">
                    </div>
                    <div class="form-group">
                        <label for="categoryDescription">Board Description</label>
                        <textarea class="form-control" id="categoryDescription" v-model="boardDescription"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="categoryDescription">Category</label>
                        <select class="form-control" v-model="selectedCategoryId" style="width: 60%" @click="populateCategoryOptions()">
                            <option v-for="(value, key) in categoryOptions" :key="key" :value="key">{{value}}</option>
                        </select>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger btn-sm" data-dismiss="modal" @click="closeModal()">Close</button>
                    <button type="button" class="btn btn-primary btn-sm font-weight-bold" @click="createBoard()">Create</button>
                </div>
            </div>
        </div>
    </div>
</template>