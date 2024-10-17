<script lang = "ts" setup>
import { onMounted, ref, watch } from 'vue';
import { useAdminPortalStore } from '@/stores/AdminPortalStore';
import { useToast } from 'vue-toastification';
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import LoadingIndicator from '@/components/LoadingIndicator.vue';


const adminPortalStore = useAdminPortalStore();
const toast = useToast();

const refresh = () => {
    adminPortalStore.getAdminPortalAppState();
}

onMounted (() => {
    adminPortalStore.getAdminPortalAppState();
});

watch(() => adminPortalStore.appStats, (newValue) => {
    if(!newValue || Object.keys(newValue).length === 0) {
        toast.error("Could not update the app state.");
        return;
    };
}, { deep: true });

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    Overview
                </span>
            </h3>
            <div class="card-toolbar">
                <ButtonWithLoadingIndicator :label="'Refresh'" :icon="'fas fa-sync'" :loading="adminPortalStore.loading_getAdminPortalAppState" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body">
            <div v-if="adminPortalStore.loading_getAdminPortalAppState" class="text-center">
            <LoadingIndicator :loading="adminPortalStore.loading_getAdminPortalAppState" />
                Loading...
            </div>
            <div v-else>
                <table class = "table table-borderless table-sm">
                    <thead>
                        <tr>
                            <th>Total categories</th>
                            <th>Total boards</th>
                            <th>Total discussions</th>
                            <th>Total posts</th>
                            <th>Total gallery items</th>
                            <th>Total users</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>{{ adminPortalStore.appStats.totalCategories }}</td>
                            <td>{{ adminPortalStore.appStats.totalBoards }}</td>
                            <td>{{ adminPortalStore.appStats.totalThreads }}</td>
                            <td>{{ adminPortalStore.appStats.totalPosts }}</td>
                            <td>{{ adminPortalStore.appStats.totalGalleryItems }}</td>
                            <td>{{ adminPortalStore.appStats.totalUsers }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>  
        </div>
    </div>
</template>