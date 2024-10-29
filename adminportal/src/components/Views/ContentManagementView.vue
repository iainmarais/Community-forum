<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import { ref, watch } from 'vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';

const contentManagementStore = useContentManagementStore();

const refresh = () => {
    //Refresh all content management data.
    contentManagementStore.getCategories();
}


import BoardsTabContent from '@/components/TabContent/BoardsTabContent.vue';
import CategoriesTabContent from '@/components/TabContent/CategoriesTabContent.vue';
import PostsTabContent from '@/components/TabContent/PostsTabContent.vue';
import TopicsTabContent from '@/components/TabContent/TopicsTabContent.vue';
import GalleryTabContent from '@/components/TabContent/GalleryTabContent.vue';

const tabs = [
    { name: 'categories', label: 'Categories', component: CategoriesTabContent },
    { name: 'boards', label: 'Boards', component: BoardsTabContent },
    { name: 'topics', label: 'Topics', component: TopicsTabContent },
    { name: 'posts', label: 'Posts', component: PostsTabContent },
    { name: 'gallery', label: 'Gallery', component: GalleryTabContent },
];

const activeTab = ref<string>('categories');

const switchTab = (tab: string) => {
    activeTab.value = tab;
}

watch(() => activeTab.value, (newValue) => {
    switch(newValue) {
        case 'categories':
            contentManagementStore.getCategories();
            activeTab.value = 'categories';
            break;
        case 'boards':
            contentManagementStore.getBoards();
            activeTab.value = 'boards';
            break;
        case 'topics':
            contentManagementStore.getTopics();
            activeTab.value = 'topics';
            break;
        case 'posts':
            contentManagementStore.getPosts();
            activeTab.value = 'posts';
            break;
        case 'gallery':
            activeTab.value = 'gallery';
            break;
        default:
            activeTab.value = 'categories';
            break;
    }
});

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    Content management
                </span>
            </h3>
            <div class="card-toolbar">
                <ul class="nav">
                    <li class="nav-item" v-for="tab in tabs" :key="tab.name" :id="tab.name">
                        <button class="nav-link" type="button" data-bs-toggle="tab" :data-bs-target="`#${tab.name}`" :aria-controls="tab.name" :aria-selected="activeTab === tab.name" :class="activeTab === tab.name ? 'active' : ''" @click.prevent="switchTab(tab.name)" href="#">
                            {{ tab.label }}
                        </button>
                    </li>
                </ul>
            </div>
        </div>
        <div class="card-body">

            <div class="tab-content">
                <div class="tab-pane fade" 
                :class="{show: activeTab === tab.name, active: activeTab === tab.name}"
                :aria-labelledby="tab.name"
                role="tabpanel"
                v-for="tab in tabs" 
                :key="tab.name" 
                :id = "tab.name">
                    <div class="container">
                        <component :is="tab.component"></component>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>