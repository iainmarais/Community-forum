<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import PageSelector from '@/components/elements/Inputs/PageSelector.vue';
import SearchBar from '@/components/elements/Inputs/SearchBar.vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import { debounce } from 'lodash';
import { ref } from 'vue';
import { useToast } from 'vue-toastification';
import LoadingIndicator from '../elements/LoadingIndicator.vue';


const toast = useToast();
const contentManagementStore = useContentManagementStore();

const searchQuery = ref("");

const refresh = () => {
    contentManagementStore.getPosts();
}

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
                    Posts
                </span>
            </h3>
            <div class="card-toolbar">
                <SearchBar v-model:searchQuery="searchQuery" :handleSearch = "search" />
                <ButtonWithLoadingIndicator style="margin-inline: 10px" :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body">
            <div v-if="!contentManagementStore.loading">
                <PageSelector :current-page-number="contentManagementStore.currentPageNumber" :totalPages="contentManagementStore.posts.totalPages" :rows-per-page="contentManagementStore.rowsPerPage" @previous-page="$emit('previous-page')" @next-page="$emit('next-page')" />
                <table class="table table-hover table-striped table-bordered" v-if="contentManagementStore.posts.rows?.length > 0">
                    <thead>
                        <tr>
                            <th>Date created</th>
                            <th>Created by</th>
                            <th>Discussion thread</th>
                            <th>Topic</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="element in contentManagementStore.posts.rows">
                            <td>{{ element.post.createdDate }}</td>
                            <td>{{ element.createdByUser.user.username }}</td>
                            <td>{{ element.thread?.thread.threadName ?? "N/A"  }}</td>
                            <td>{{ element.thread?.topic?.topic.topicName ?? "N/A"  }}</td>
                            <td>
                                <button class = "btn btn-sm btn-primary" style="margin-inline: 10px;" >
                                    <i class="fas fa-edit"></i>
                                    Edit post
                                </button>
                                <button class = "btn btn-sm btn-primary" style="margin-inline: 10px;" >
                                    <i class="fas fa-eye"></i>
                                    View post
                                </button>
                                <button class = "btn btn-sm btn-outline-danger" style="margin-inline: 10px;" >
                                    <i class="fas fa-xmark"></i>
                                    Delete
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div v-else>
                    <p>No posts to show.</p>
                </div>
            </div>
            <div class="card-body" v-else>
                <LoadingIndicator :loading="true" />
            </div>
        </div>
    </div>
</template>