<script lang = "ts" setup>
import { onBeforeUnmount, onMounted, ref, watch } from 'vue';
import { useAdminPortalStore } from '@/stores/AdminPortalStore';
import { useAppContextStore } from '@/stores/AppContextStore';
import { useToast } from 'vue-toastification';
import type { AdminPortalStats } from '@/Dto/AdminPortal/AdminPortalAppState';


const adminPortalStore = useAdminPortalStore();
const appContextStore = useAppContextStore();
const toast = useToast();

const adminPortalStats = ref<AdminPortalStats>();

const refresh = () => {
    adminPortalStore.getAdminPortalAppState();
    adminPortalStats.value = adminPortalStore.adminPortalAppState.adminPortalStats;
}

onMounted (() => {
    if(!appContextStore.appStats) {
        toast.error("Could not retrieve initial admin portal stats.");
    }
    adminPortalStats.value = appContextStore.appStats;
    //Update this from the admin portal store every 5 seconds
    const updateInterval = setInterval(() => {
        adminPortalStore.getAdminPortalAppState();
        adminPortalStats.value = adminPortalStore.adminPortalAppState.adminPortalStats;
    }, 5000);
    
    onBeforeUnmount(() => {
        clearInterval(updateInterval);
    });
});

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
                <button class="btn btn-primary btn-sm" @click.prevent="refresh()">Refresh</button>
            </div>
        </div>
        <div class="card-body">
            <div v-if="adminPortalStats !== undefined">
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
                    <tbody v-for="stat in adminPortalStats">
                        <tr>
                            <td>{{ stat }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</template>