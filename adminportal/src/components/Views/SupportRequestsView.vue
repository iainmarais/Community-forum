<script lang = "ts" setup>
//Technical support requests view
// 
//Support requests will be sent through the forum client portal, and will show up here for the admin team to handle.
//Important props include date, user, type and message.
//This is different from the type of tech support people offer through forums since these tech support requests are for issues pertaining to the forum platform.
import { onMounted, ref, watch } from 'vue';
import { useSupportRequestsStore } from '@/stores/SupportRequestsStore';

const SupportRequestsStore = useSupportRequestsStore();

onMounted(() => {
    SupportRequestsStore.getSupportRequests();
});

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label fw-bolder text-dark">Support Requests</span>
            </h3>
        </div>
        <div class="card-body pt-0">
            <div class="table-responsive">
                <table v-if="SupportRequestsStore.requests" class="table table-row-dashed table-row-gray-300 align-middle gs-0 gy-4">
                    <thead>
                        <tr class="fw-bolder fs-6 text-gray-800">
                            <th class="min-w-125px">Date</th>
                            <th class="min-w-125px">User</th>
                            <th class="min-w-125px">Type</th>
                            <th class="min-w-125px">Message</th>
                        </tr>
                    </thead>
                    <tbody class="fs-6">
                        <tr v-for="element in SupportRequestsStore.requests.rows" :key="element.request.requestId">
                            <td>{{ element.request.createdDate }}</td>
                            <td>{{ element.request.createdByUser }}</td>
                            <td>{{ element.request.supportRequestTitle }}</td>
                            <td>{{ element.request.supportRequestContent }}</td>
                        </tr>
                    </tbody>
                </table>
                <div v-else>
                    <div class="text-center">
                        <span class="text-muted fw-bold fs-6">No support requests found.</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>