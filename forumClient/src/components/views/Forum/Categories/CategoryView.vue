<script lang="ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { CategoryBasicInfo, CategoryFullInfo } from '@/Dto/app/CategoryInfo';
import UserService from '@/services/UserService';
import { useCategoryStore } from '@/stores/Categories/CategoryStore';
import { onMounted, ref, watch } from 'vue';
import { useToast } from 'vue-toastification';
import dayjs from 'dayjs';
import DateUtils from '@/components/utils/DateUtils';

const toast = useToast();
const categoryStore = useCategoryStore();
const categories = ref<CategoryBasicInfo[]>();
const categoryVisibility = ref<Record<string, boolean>>({}); // Map of category visibility

const categoryButtonTooltip = "Toggle visibility of this category. Categories with no topics will not be shown by default.";

const createdByUser = ref<UserBasicInfo>();

const setCategoryVisibleButtonLabel = (categoryId: string) => {
    return categoryVisibility.value[categoryId] ? "Hide" : "Show";
}

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

const toggleCategoryVisibility = (categoryId: string) => {
    categoryVisibility.value[categoryId] = !categoryVisibility.value[categoryId];
};

const changeCategoryVisibilityButtonIcon = (categoryId: string) => {
    return categoryVisibility.value[categoryId] ? "fas fa-eye-slash" : "fas fa-eye";
}

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
};
</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Categories</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm"><i class="fas fa-plus"></i>Create new</button>
            </div>
        </div>
        <div class="card-body">
            <div class="card card-custom" v-for="category in categories" :key="category.categoryId">
                <div class="card-header border-0 pt-7">
                    <h3 class="card-title align-items-start flex-column">
                        <span class="card-label font-weight-bolder text-dark075 font-size-h5">{{ category.categoryName }}</span>
                    </h3>
                    <div class="card-toolbar">
                        <button id="categoryVisibilityButton" class="btn btn-outline-primary btn-sm" data-toggle="categoryButtonTooltip" data-placement="left" :title="categoryButtonTooltip" @click="toggleCategoryVisibility(category.categoryId)">
                            <i :class=changeCategoryVisibilityButtonIcon(category.categoryId)></i>{{ setCategoryVisibleButtonLabel(category.categoryId) }}
                        </button>
                    </div>
                </div>
                <div class="card-body" :id="'categoryBody-' + category.categoryId" v-show="categoryVisibility[category.categoryId]">
                    <div class="card card-custom" v-for="topic in category.topics" :key="topic.topicId">
                        <div class="card-body">
                            <div class="row-sm d-flex">
                                <div class="col-sm-9">
                                    <h5 class="card-title align-items-start flex-column">
                                        <p>
                                            <span class="card-label font-weight-bolder text-dark075 font-size-h6">
                                                {{ topic.topicName }}
                                            </span>
                                        </p>
                                    </h5>
                                    <p>
                                        <span>
                                            {{ topic.description }}
                                        </span>
                                    </p>
                                </div>
                                <div class="col-sm-3">
                                    <h5 class="card-title align-items-start flex-column">
                                        <p>
                                            <span class="card-label font-weight-bolder text-dark075 font-size-h6">
                                                {{ DateUtils.formatDate(topic.createdDate) }}
                                            </span>
                                        </p>
                                    </h5>
                                    <p>
                                        <span>
                                            {{ getUserInfo(topic.createdByUserId)?.username ?? "" }}
                                        </span>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>  
                </div>
            </div>
        </div>
    </div>
</template>
