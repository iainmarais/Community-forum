<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import { watch } from 'vue';
import { useToast } from 'vue-toastification';
import CreateTopicModal from '@/components/modals/CreateTopicModal.vue';
import { Modal } from 'bootstrap';

const contentManagementStore = useContentManagementStore();
const toast = useToast();

const refresh = () => {
    contentManagementStore.getTopics();
}

watch(() => contentManagementStore.result_deleteTopic, (newValue) => {
    if(newValue) {
        toast.success("Topic deleted successfully");
        contentManagementStore.getTopics();
    }
});

const deleteTopic = (topicId: string) => {
    if (topicId.length == 0 || topicId == null) {
        toast.error("Topic Id can't be null or empty");
        return;
    }
    contentManagementStore.deleteTopic(topicId);
}

const openCreateTopicModal = () => {
    var createTopicModal = document.getElementById("CreateTopicModal");
    var modal = new Modal(createTopicModal);
    modal.show();
}

</script>

<template>
    <div class="card card-custom">
        <div class="card-header">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    Topics
                </span>
            </h3>
            <div class="card-toolbar">
                <!--Todo: add search function-->
                <button class="btn btn-primary btn-sm font-weight-bold" style="margin-inline: 10px" @click="openCreateTopicModal()"><i class="fas fa-plus"></i>Create Topic</button>
                <ButtonWithLoadingIndicator style="margin-inline: 10px" :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body" v-if="!contentManagementStore.loading">
            <table class="table table-hover table-striped table-bordered" v-if="contentManagementStore.topics.rows.length > 0">
                <thead>
                    <tr>
                        <th>Topic Name</th>   
                        <th>Description</th>   
                        <th>Created By</th>   
                        <th>Actions</th>   
                    </tr>
                </thead>
                    <tbody>
                    <tr v-for="element in contentManagementStore.topics.rows">
                        <td> {{ element.topic.topicName ?? "N/A" }} </td>
                        <td> {{ element.topic.description ?? "N/A" }}</td>
                        <td> {{ element.createdByUser.user.username ?? "N/A" }}</td>
                        <td> 
                            <button style="margin-inline: 10px" class ="btn btn-sm btn-primary"><i class="fas fa-edit"></i>Edit</button>
                            <button style="margin-inline: 10px" class ="btn btn-sm btn-primary"><i class="fas fa-eye"></i>View</button>
                            <button style="margin-inline: 10px" class="btn btn-sm btn-danger" @click = deleteTopic(element.topic.topicId)><i class="fas fa-xmark"></i>Delete</button>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div v-else>
                <p>No topics found</p>
            </div>
        </div>
        <div class="card-body" v-else>
            <LoadingIndicator :loading="true" />
        </div>
    </div>
<CreateTopicModal />
</template>