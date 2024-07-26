<script lang = "ts" setup>
import { useCategoryStore } from '@/stores/Categories/CategoryStore';
import { onMounted, ref, watch } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import { useToast } from 'vue-toastification';
import CreateNewTopicModal from '@/components/modals/CreateNewTopicModal.vue';
import UserService from '@/services/UserService';
import DateUtils from '@/components/utils/DateUtils';
import type { UserBasicInfo } from '@/Dto/UserInfo';

const toast = useToast();
const router = useRouter();
const route = useRoute();

const categoryId = ref<string>(route.params.categoryId as string);
const categoryStore = useCategoryStore();
const createdByUser = ref<UserBasicInfo>();

onMounted(() => {
    categoryStore.getCategoryFullInfo(categoryId.value);
});

const createNewTopic = () => {
    $("#createTopicModal").modal("show");
}

const handleTopicCreated = () => {
    categoryStore.getCategoryFullInfo(categoryId.value);
}

const goBack = () => {
    router.push({name: "overview"});
}

const getTopicName = (topicId:string) => {
    var topic = categoryStore.category?.topics?.find((topic) => topic.topicId === topicId);
    return topic?.topicName ?? "";
}

const viewTopic = (topicId: string) => {
    toast.info("Viewing topic: " + getTopicName(topicId));
    router.push({ name: "ViewTopic", params: { topicId: topicId } });
}

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
}
</script>

<template>
    <div class="card card-custom" v-if="!categoryStore.loading_getCategoryFullInfo">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Category: {{ categoryStore.category.category.categoryName }}</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm m-1" @click="goBack()"><i class="fas fa-arrow-left"></i>Back</button>
                <button class="btn btn-primary btn-sm m-1" @click="createNewTopic"><i class="fas fa-plus"></i>Create new topic</button>
            </div>
        </div>
        <div class="card-body">
            <table class="table table-borderless table-sm" >
                <tr v-for="topic in categoryStore.category.topics" :key="topic.topicId">
                <td>
                    <div class="d-flex align-items-center">
                        <div class="symbol symbol-50px me-5">
                            <span class="symbol-label bg-light">
                                <i class="fas fa-folder" style="font-size: 30px"></i>
                            </span>
                        </div>
                        <div class="ml-3">
                            <div class="text-dark-75 font-weight-bolder font-size-lg mb-0">
                                <a class="text-dark font-weight-bolder text-hover-primary mb-1 font-size-lg" @click="viewTopic(topic.topicId)"> {{ topic.topicName }} </a>
                            </div>
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block">
                                    {{ topic.description }}
                                </span>
                            </div>
                        </div>
                        <div class="ml-auto">
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block"> Total threads: {{ topic.numTotalThreads }} </span>
                            </div>
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block"> New threads: {{ topic.numNewThreads }} </span>
                            </div>
                        </div>
                        <div class="ml-auto">
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block"> Creator: {{ getUserInfo(topic.createdByUserId)?.username }} </span>
                            </div> 
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block"> Created: {{ DateUtils.formatDate(topic.createdDate) }} </span>
                            </div>
                        </div>
                    </div>
                </td>
                </tr>
            </table>
        </div>
    </div>
    <LoadingIndicator :loading="categoryStore.loading_getCategoryFullInfo" />
    <CreateNewTopicModal :selectedCategoryId="categoryId" @topicCreated="handleTopicCreated" />
</template>