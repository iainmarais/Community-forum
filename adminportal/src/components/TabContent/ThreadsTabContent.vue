<script lang = "ts" setup>
import PageSelector from '@/components/elements/Inputs/PageSelector.vue';
import SearchBar from '@/components/elements/Inputs/SearchBar.vue';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import { debounce } from 'lodash';
import { ref } from 'vue';
import { useToast } from 'vue-toastification';
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';

const contentManagementStore = useContentManagementStore();
const toast = useToast();

const refresh = () => {
    //Get threads from the server.
}

const searchQuery = ref("");

const search = debounce((query: string) => {
    contentManagementStore.searchQuery = query;
    contentManagementStore.getPosts();   
}, 300);



</script>

<template>
    <div class="card card-custom">
        <div class="card-header">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    Threads
                </span>
            </h3>
            <div class="card-toolbar">
                <SearchBar v-model:searchQuery="searchQuery" :handleSearch = "search" />
                <ButtonWithLoadingIndicator style="margin-inline: 10px" :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>            
        </div>
        <div class ="card-body">
            <p>Coming soon...</p>
        </div>
    </div>

</template>