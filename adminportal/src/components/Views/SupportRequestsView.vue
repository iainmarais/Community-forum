<script lang = "ts" setup>
//Technical support requests view
// 
//Support requests will be sent through the forum client portal, and will show up here for the admin team to handle.
//Important props include date, user, type and message.
//This is different from the type of tech support people offer through forums since these tech support requests are for issues pertaining to the forum platform.
import { onMounted, ref, watch } from 'vue';
import { useSupportRequestsStore } from '@/stores/SupportRequestsStore';
import PageSelector from '@/components/elements/Inputs/PageSelector.vue';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import SearchBar from '@/components/elements/Inputs/SearchBar.vue';
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import { debounce } from 'lodash';
import dayjs from 'dayjs';

const supportRequestsStore = useSupportRequestsStore();

const refresh = () => {
    //Todo: update this function to get support requests from the server.
}

const formatDate = (date: Date) => {
    return dayjs(date).format('DD/MM/YYYY');
}

const searchQuery = ref("");

const search = debounce((query: string) => {
    supportRequestsStore.searchQuery = query;
    supportRequestsStore.getSupportRequests();   
}, 300);

onMounted(() => {
    supportRequestsStore.getSupportRequests();
});

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label fw-bolder text-dark">Support Requests</span>
            </h3>
            <div class="card-toolbar">
                <SearchBar v-model:searchQuery="searchQuery" :handleSearch = "search" />
                <ButtonWithLoadingIndicator style="margin-inline: 10px" :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>            
        </div>
        <div class="card-body pt-0">
            <PageSelector :current-page-number="supportRequestsStore.currentPageNumber" :totalPages="supportRequestsStore.requests.totalPages" :rows-per-page="supportRequestsStore.rowsPerPage" @previous-page="$emit('previous-page')" @next-page="$emit('next-page')" />
            <div class="row">
                <div class="col-md-12">
                    <div class="text-center">
                        <div v-if="!supportRequestsStore.loading_getRequests">
                            <table class="table table-hover table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th>Created</th>
                                        <th>Creator</th>
                                        <th>Title</th>
                                        <th>Message</th>
                                        <th colspan="2">Triage</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="element in supportRequestsStore.requests.rows" :key="element.request.requestId">
                                        <td>{{ formatDate(element.request.createdDate) }}</td>
                                        <td>{{ element.createdByUser.user.username }}</td>
                                        <td>{{ element.request.supportRequestTitle }}</td>
                                        <td>{{ element.request.supportRequestContent }}</td>
                                        <td> {{ element.request.triageStatus }}</td>
                                        <td>{{ element.request.triageType }}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <LoadingIndicator v-else :loading="true" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>