<script lang = "ts" setup>
import type { CategoryBasicInfo } from '@/Dto/app/CategoryInfo';
import { computed, ref, watch, type PropType } from 'vue';
import TopicItem from './TopicItem.vue';
import { useToast } from 'vue-toastification';
import { useCategoryStore } from '@/stores/Categories/CategoryStore';
import CreateNewTopicModal from '@/components/modals/CreateNewTopicModal.vue';
import LoadingIndicator from './LoadingIndicator.vue';

const toast = useToast();   
const categoryStore = useCategoryStore();
const props = defineProps({
    category: {
        type: Object as PropType<CategoryBasicInfo>,
        required: true
    }
});

const categoryButtonTooltip = "Toggle visibility of this category. Categories with no topics will not be shown by default.";
const categoryVisibility = ref(false);
const toggleCategoryVisibility = () => {
    categoryVisibility.value = !categoryVisibility.value;
};

const setCategoryVisibleButtonLabel = () => {
    return categoryVisibility.value ? "Hide" : "Show";
}

const changeCategoryVisibilityButtonIcon = () => {
    return categoryVisibility.value ? "fas fa-eye-slash" : "fas fa-eye";
}

const createNewTopic = () => {
    openCreateTopicModal();
}

const openCreateTopicModal = () => {
    categoryStore.selectedCategoryId = props.category.categoryId;
    $("#createTopicModal").modal("show");
}

const categoryLoading = computed(() => categoryStore.loading_selectedCategory.get(props.category.categoryId) || false);

watch(() => categoryVisibility.value, (newValue) => {
    if (newValue == true) {
        categoryStore.getSelectedCategory(props.category.categoryId);
    }
});

</script>

<template>
     <table class="table table-borderless table-vertical-center">
        <tbody>
            <tr>
                <td>
                    <div class="d-flex align-items-center">
                        <div class="symbol symbol-50 flex-shrink-0">
                            <div class="symbol-label">
                                <i class="fas fa-user fa-lg"></i>
                            </div>
                        </div>
                        <div class="ml-3">
                            <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">
                                {{ props.category.categoryName }}
                            </div>
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block">{{ props.category.categoryDescription }}</span>
                            </div>
                        </div>
                        <div class="ml-auto">
                            <button id="categoryVisibilityButton" class="btn btn-outline-primary btn-sm m-1" data-toggle="categoryButtonTooltip" data-placement="left" :title="categoryButtonTooltip" @click="toggleCategoryVisibility(props.category.categoryId)">
                                <i :class=changeCategoryVisibilityButtonIcon()></i>{{ setCategoryVisibleButtonLabel() }}
                            </button>
                            <button id="createTopicButton" class="btn btn-outline-primary btn-sm m-1" @click="createNewTopic">
                                <i class="fas fa-plus"></i> Create New Topic
                            </button>
                        </div>
                    </div>
                    <div v-if="!categoryLoading">
                        <div class="card card-custom pt-4" :id="'categoryBody-' +  props.category.categoryId" v-show="categoryVisibility">
                            <div v-if="props.category.topics.length === 0">
                                <div class="card-body pt-3">
                                    <span>No topics in this category.</span>
                                </div>
                            </div>
                            <div v-else>
                                <TopicItem v-for="topic in props.category.topics" :key="topic.topicId" :topic="topic" />
                            </div>
                        </div>
                    </div>
                    <div v-else>
                        <LoadingIndicator :loading="categoryLoading" />
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
<CreateNewTopicModal :selected-category-id="categoryStore.selectedCategoryId"/>
</template>