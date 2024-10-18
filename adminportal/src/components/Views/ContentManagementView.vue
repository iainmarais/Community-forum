<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import { ref, watch } from 'vue';

const refresh = () => {
    //Todo: build out.
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
            activeTab.value = 'categories';
            break;
        case 'boards':
            activeTab.value = 'boards';
            break;
        case 'topics':
            activeTab.value = 'topics';
            break;
        case 'posts':
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
                <ButtonWithLoadingIndicator :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body">
            <ul class="nav">
                <li class="nav-item" v-for="tab in tabs" :key="tab.name" :id="tab.name">
                    <button class="nav-link" type="button" data-bs-toggle="tab" :data-bs-target="`#${tab.name}`" :aria-controls="tab.name" :aria-selected="activeTab === tab.name" :class="activeTab === tab.name ? 'active' : ''" @click.prevent="switchTab(tab.name)" href="#">
                        {{ tab.label }}
                    </button>
                </li>
            </ul>
            <div class="tab-content">
                <div class="tab-pane fade" 
                :class="{show: activeTab === tab.name, active: activeTab === tab.name}"
                :aria-labelledby="tab.name"
                role="tabpanel"
                v-for="tab in tabs" 
                :key="tab.name" 
                :id = "tab.name">
                    <component :is="tab.name" v-if="activeTab === tab.name" />
                </div>
            </div>
        </div>
    </div>
</template>