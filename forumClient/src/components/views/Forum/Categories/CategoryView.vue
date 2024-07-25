<script lang="ts" setup>
import type { CategoryBasicInfo } from '@/Dto/app/CategoryInfo';
import { useCategoryStore } from '@/stores/Categories/CategoryStore';
import { onMounted, ref, watch } from 'vue';
import { useToast } from 'vue-toastification';
import CategoryItem from '@/components/elements/CategoryItem.vue';
import CreateCategoryModal from '@/components/modals/CreateCategoryModal.vue';


const toast = useToast();
const categoryStore = useCategoryStore();
const categories = ref<CategoryBasicInfo[]>();
const categoryVisibility = ref<Record<string, boolean>>({}); // Map of category visibility

onMounted(() => {
    categoryStore.getCategories();
});

watch(() => categoryStore.categories, (newCategories) => {
    if (newCategories.length === 0) return;
    categories.value = newCategories;
    newCategories.forEach(category => {
        categoryVisibility.value[category.categoryId] = false; // Initialize visibility state
    });
});

const createNewCategory = () => {
    openCreateCategoryModal();
}

const openCreateCategoryModal = () => {
    $("#createCategoryModal").modal("show");
}

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-3">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Categories</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm" @click="createNewCategory"><i class="fas fa-plus"></i>Create new</button>
            </div>
        </div>
        <div class="card-body">
            <CategoryItem v-for="category in categories" :key="category.categoryId" :category="category" />
        </div>
    </div>
<CreateCategoryModal  />
</template>
