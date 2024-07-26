<script lang="ts" setup>
import type { CategoryBasicInfo } from "@/Dto/app/CategoryInfo";
import { useCategoryStore } from "@/stores/Categories/CategoryStore";
import { onMounted, ref, watch } from "vue";
import { useToast } from "vue-toastification";
import CreateCategoryModal from "@/components/modals/CreateCategoryModal.vue";
import { useRouter } from "vue-router";

const router = useRouter();
const toast = useToast();
const categoryStore = useCategoryStore();
const categories = ref<CategoryBasicInfo[]>();
const categoryVisibility = ref<Record<string, boolean>>({}); // Map of category visibility
const categoryName = ref<string>("");

onMounted(() => {
  categoryStore.getCategories();
});

const viewCategory = (categoryId: string) => {
    toast.info("Viewing category: " + getCategoryName(categoryId));
    router.push({ name: "ViewCategory", params: { categoryId: categoryId } });
}

const getCategoryName = (categoryId: string) => {
    var category = categories.value?.find((category) => category.categoryId === categoryId);
    return category?.categoryName ?? "";
}

watch(
  () => categoryStore.categories,
  (newCategories) => {
    if (newCategories.length === 0) return;
    categories.value = newCategories;
    newCategories.forEach((category) => {
      categoryVisibility.value[category.categoryId] = false; // Initialize visibility state
    });
  }
);
</script>

<template>
  <div class="card card-custom">
    <div v-if="categoryStore.categories?.length > 0" class="card-body">
      <table class="table table-striped table-hover table-sm">
        <tr v-for="category in categories" :key="category.categoryId">
          <td>
            <div class="d-flex align-items-center">
              <div class="symbol symbol-50px me-5">
                <span class="symbol-label bg-light">
                  <i class="fas fa-folder" style="font-size: 30px"></i>
                </span>
              </div>
              <div class="ml-3">
                <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">
                    <a class="text-dark font-weight-bolder text-hover-primary mb-1 font-size-lg" @click="viewCategory(category.categoryId)"> {{ category.categoryName }} </a>
                </div>
                <div>
                  <span class="text-muted font-weight-bold text-muted d-block">
                    {{ category.categoryDescription }}
                  </span>
                </div>
              </div>
            </div>
          </td>
        </tr>
      </table>
    </div>
    <div v-else class="card-body">
      <p>No categories found</p>
    </div>
  </div>
  <CreateCategoryModal />
</template>
