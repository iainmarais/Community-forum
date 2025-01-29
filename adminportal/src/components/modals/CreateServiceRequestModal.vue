<script lang = "ts" setup>
import { onMounted, ref, watch } from 'vue';
import { useServiceRequestsStore } from '@/stores/ServiceRequestsStore';
import { useToast } from 'vue-toastification';
import type { CreateAdminPortalRequest } from '@/Dto/AdminPortal/CreateAdminPortalRequest';
import { Modal } from 'bootstrap';

const toast = useToast();
const serviceRequestsStore = useServiceRequestsStore();

const serviceRequestTitle = ref<string>("");
const serviceRequestContent = ref<string>("");

const closeModal = () => {
    var modal = document.getElementById("CreateServiceRequestModal");
    var modalInstance = Modal.getInstance(modal);
    modalInstance?.hide();
}

const createServiceRequest = () => {
    if(serviceRequestContent.value.length == 0 || serviceRequestTitle.value.length == 0) {
        toast.error("Please provide a valid title and message");
        return;
    }
    const req: CreateAdminPortalRequest = {
        supportRequestTitle: serviceRequestTitle.value,
        supportRequestContent: serviceRequestContent.value
    }
    serviceRequestsStore.createServiceRequest(req);
}

watch(() => serviceRequestsStore.result_createServiceRequestSuccess, (newValue) => {
    if(newValue) {
        toast.success("Service request created successfully");
        serviceRequestsStore.getSupportRequests();
        closeModal();
    }
})

</script>

<template>
    <div class = "modal fade stackable" id="CreateServiceRequestModal" tabindex="-1" role="dialog" aria-labelledby="CreateServiceRequestModal" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="CreateTopicModalLabel">Create service request</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button>                
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="serviceRequestTitle">Title</label>
                        <textarea class="form-control" id="serviceRequestTitle" v-model="serviceRequestTitle"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="serviceRequestContent">Message</label>
                        <textarea class="form-control" id="serviceRequestContent" v-model="serviceRequestContent"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger btn-sm" data-dismiss="modal" @click="closeModal()">Close</button>
                    <button type="button" class="btn btn-primary btn-sm font-weight-bold" @click="createServiceRequest()">Create</button>
                </div>
            </div>
        </div>
    </div>
</template>