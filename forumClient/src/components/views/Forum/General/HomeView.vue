<script lang = "ts" setup>
import ForumStatsView from '@/components/views/Forum/General/ForumStatsView.vue';
import CategoryList from '@/components/views/Forum/Categories/CategoryList.vue';
import { onMounted, ref, watch } from 'vue';
import CreateCategoryModal from '@/components/modals/CreateCategoryModal.vue';
import { useCategoryStore } from '@/stores/Categories/CategoryStore';

const categoryStore = useCategoryStore();
const createNewCategory = () => {
    $("#createCategoryModal").modal("show");
}

onMounted(() => {
    categoryStore.getCategories();

});
</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Forum - Home</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm" @click="createNewCategory"><i class="fas fa-plus"></i>Create new category</button>
            </div>
        </div>
        <div class="card-body">
            <CategoryList />
            <ForumStatsView />
        </div> 
    </div>
<CreateCategoryModal  />    
</template>