<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import { onMounted, ref, watch } from 'vue';
import CreateCategoryModal from '@/components/modals/CreateCategoryModal.vue';
import { Modal } from 'bootstrap';
import { debounce, result } from 'lodash';
import { useToast } from 'vue-toastification';
import PageSelector from '@/components/elements/Inputs/PageSelector.vue';
import SearchBar from '@/components/elements/Inputs/SearchBar.vue';

const searchQuery = ref("");

const toast = useToast();
const contentManagementStore = useContentManagementStore();

const openCreateCategoryModal = () => {
    var createCategoryModal = document.getElementById("CreateCategoryModal");    
    var modal = new Modal(createCategoryModal);
    modal.show();
}

const refresh = () => {
    contentManagementStore.getCategories();
}

const search = debounce((query: string) => {

//Update the search query from the user's input, then trigger the getUserInfo function in the store to pull an updated list of users.
//Need a way to wait until the user has finished entering their query before executing this function.
    contentManagementStore.searchQuery = query;
    contentManagementStore.getCategories();   
}, 300);

watch(() =>contentManagementStore.result_deleteCategory, (newValue) => {
    if (newValue) {
        contentManagementStore.getCategories();
    }
});

const deleteCategory = (categoryId: string) => {
    if(categoryId.length == 0) {
        toast.error("Category Id can't be empty");
        return
    }
    contentManagementStore.deleteCategory(categoryId);
}

const viewCategory = (categoryId: string) => {
    //Todo: build out.
}

onMounted(() => {
    contentManagementStore.getCategories();
});

//Watch the result of getCategories.
watch(() => contentManagementStore.result_getCategories, (newValue) => {
    if(newValue) {
        contentManagementStore.getCategories();
    }
})

</script>

<template>
    <div class="card card-custom">
        <div class="card-header">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    Categories
                </span>
            </h3>
            <div class="card-toolbar">
                <SearchBar v-model:searchQuery="searchQuery" :handleSearch = "search" />
                <button class="btn btn-sm btn-primary" style="margin-inline: 10px;" @click="openCreateCategoryModal()"><i class="fas fa-plus"></i>Create Category</button>
                <ButtonWithLoadingIndicator style="margin-inline: 10px;" :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body" v-if="!contentManagementStore.loading">
            <PageSelector :current-page-number="contentManagementStore.currentPageNumber" :totalPages="contentManagementStore.categories.totalPages" :rows-per-page="contentManagementStore.rowsPerPage" @previous-page="$emit('previous-page')" @next-page="$emit('next-page')" />
            <table class="table table-hover table-striped table-bordered">
                <thead>
                    <tr>
                        <th>Category Name</th>   
                        <th>Description</th>   
                        <th>Number of Boards</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                    <tbody>
                    <tr v-for="element in contentManagementStore.categories.rows">
                        <td> {{ element.category.categoryName ?? "N/A" }} </td>
                        <td> {{ element.category.categoryDescription ?? "N/A" }}</td>
                        <td> {{ element.boards.length ?? 0 }}</td>
                        <td>
                            <button class ="btn btn-sm btn-primary" style="margin-inline: 10px"><i class="fas fa-edit"></i>Edit</button>
                            <button class ="btn btn-sm btn-primary" style="margin-inline: 10px" @click="viewCategory(element.category.categoryId)"><i class="fas fa-eye"></i>View</button>
                            <button class="btn btn-sm btn-outline-danger" style="margin-inline: 10px" @click="deleteCategory(element.category.categoryId)">
                                <i class="fas fa-xmark"></i>
                                Delete
                            </button>                            
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="card-body" v-else>
            <LoadingIndicator :loading="true" />
        </div>
    </div>
<CreateCategoryModal />
</template>