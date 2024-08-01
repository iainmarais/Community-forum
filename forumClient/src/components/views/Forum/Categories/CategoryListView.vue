<script lang="ts" setup>
import type { CategoryBasicInfo } from "@/Dto/app/CategoryInfo";
import { useCategoryStore } from "@/stores/Categories/CategoryStore";
import { onMounted, ref, watch } from "vue";
import CreateCategoryModal from "@/components/modals/CreateCategoryModal.vue";
import CategoryItem from "./CategoryItem.vue";


const categoryStore = useCategoryStore();
const categories = ref<CategoryBasicInfo[]>();
const categoryVisibility = ref<Record<string, boolean>>({}); // Map of category visibility


onMounted(() => {
  categoryStore.getCategories();
});

watch(
  () => categoryStore.categories,
  (newCategories) => {
    if (newCategories.length === 0) return;
    categories.value = newCategories;
    newCategories.forEach((category) => {
      categoryVisibility.value[category.category.categoryId] = false; // Initialize visibility state
    });
  }
);
</script>

<template>
  <div class="card card-custom">
    <div class="card-body" v-if="categoryStore.categories?.length > 0">
        <table class="table table-borderless table-sm">
            <CategoryItem :category="category" v-for="category in categories" :key="category.category.categoryId" />
        </table>
    </div>
    <div v-else class="card-body">
      <p>No categories found</p>
    </div>
  </div>
  <CreateCategoryModal />
</template>
