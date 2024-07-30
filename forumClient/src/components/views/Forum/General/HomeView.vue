<script lang = "ts" setup>
import ForumStatsView from '@/components/views/Forum/General/ForumStatsView.vue';
import CategoryList from '@/components/views/Forum/Categories/CategoryListView.vue';
import { onMounted, ref, watch } from 'vue';
import CreateCategoryModal from '@/components/modals/CreateCategoryModal.vue';
import { useCategoryStore } from '@/stores/Categories/CategoryStore';
import { useAppContextStore } from '@/stores/AppContextStore';
import { useToast } from 'vue-toastification';
import { useRouter } from 'vue-router';

const categoryStore = useCategoryStore();
const appContextStore = useAppContextStore();
const toast = useToast();
const router = useRouter();

const createNewCategory = () => {
    $("#createCategoryModal").modal("show");
}

onMounted(() => {
    categoryStore.getCategories();

});

//Sorry mate, you're not allowed to be here without permission. Try knocking on the front door first :)
watch(() => appContextStore.loggedInUser, newValue => {
    if (!newValue) {
        toast.error("You must be logged in to view this page");
        router.push({name: "login"});
    }
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