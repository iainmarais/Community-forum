<script lang = "ts" setup>
import { useCategoryStore } from '@/stores/Categories/CategoryStore';
import { onMounted, ref, watch } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import { useToast } from 'vue-toastification';
import CreateNewBoardModal from '@/components/modals/CreateNewBoardModal.vue';
import { useAppContextStore } from '@/stores/AppContextStore';
import type { CategoryFullInfo } from '@/Dto/app/CategoryInfo';
import BoardItem from '@/components/views/Forum/Boards/BoardItem.vue';

const categoryStore = useCategoryStore();
const appContextStore = useAppContextStore();

const toast = useToast();
const router = useRouter();
const route = useRoute();

const categoryId = ref<string>(route.params.categoryId as string || "");
const category = ref<CategoryFullInfo>();

onMounted(() => {
    if(categoryId.value) {
        categoryStore.getSelectedCategory(categoryId.value);        
    }
});

watch(() => route.params.categoryId, (newCategoryId) => {
    categoryId.value = newCategoryId as string || "";
    //Redirect the user to the home page in the event of this value being "home"
    if(categoryId.value === "home") {
        router.push({name: "overview"}); 
    }
    //else if it is valid, get the associated category.
    if(categoryId.value) {
        categoryStore.getSelectedCategory(categoryId.value);
    }
});

watch(() => appContextStore.loggedInUser, newValue => {
    if (!newValue) {
        toast.error("You must be logged in to view this page");
        router.push({name: "login"});
    }
});

watch (() => categoryStore.selectedCategory, (newCategory) => {
    if(newCategory) {
        category.value = newCategory;
    }
})

const createBoard = () => {
    $("#createBoardModal").modal("show");
}

const handleBoardCreated = () => {
    categoryStore.getSelectedCategory(categoryId.value);
}

const goBack = () => {
    router.push({name: "home"});
}

</script>

<template>
    <div class="card card-custom" v-if="!categoryStore.loading_getCategoryFullInfo">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Boards for Category: {{ category?.category.categoryName }}</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm m-1" @click="goBack()"><i class="fas fa-arrow-left"></i>Back</button>
                <button class="btn btn-primary btn-sm m-1" @click="createBoard"><i class="fas fa-plus"></i>Create new board</button>
            </div>
        </div>
        <div class="card-body">
            <BoardItem v-for="board in category?.boards" :key="board.board.boardId" :board="board" />
        </div>
    </div>
    <LoadingIndicator :loading="categoryStore.loading_getCategoryFullInfo" />
    <CreateNewBoardModal :selectedCategoryId="categoryId" @boardCreated="handleBoardCreated" />
</template>