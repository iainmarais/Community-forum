<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import { onMounted, ref, watch } from 'vue';
import CreateCategoryModal from '@/components/modals/CreateCategoryModal.vue';
import { Modal } from 'bootstrap';

const contentManagementStore = useContentManagementStore();

const openCreateCategoryModal = () => {
    var createCategoryModal = document.getElementById("CreateCategoryModal");    
    var modal = new Modal(createCategoryModal);
    modal.show();
}

const refresh = () => {
    contentManagementStore.getCategories();
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
                <!--Todo: add search function-->
                <button class="btn btn-sm btn-primary" style="margin-inline: 10px;" @click="openCreateCategoryModal()"><i class="fas fa-plus"></i>Create Category</button>
                <ButtonWithLoadingIndicator style="margin-inline: 10px;" :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body" v-if="!contentManagementStore.loading">
            <table class="table table-hover table-striped table-bordered">
                <thead>
                    <tr>
                        <th>Category Name</th>   
                        <th>Description</th>   
                        <th>Number of Boards</th>   
                    </tr>
                </thead>
                    <tbody>
                    <tr v-for="element in contentManagementStore.categories.rows">
                        <td> {{ element.category.categoryName ?? "N/A" }} </td>
                        <td> {{ element.category.categoryDescription ?? "N/A" }}</td>
                        <td> {{ element.boards.length ?? 0 }}</td>
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