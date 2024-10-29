<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import CreateBoardModal from '@/components/modals/CreateBoardModal.vue';
import { Modal } from 'bootstrap';
import { useToast } from 'vue-toastification';
import { watch } from 'vue';

const toast = useToast();

const contentManagementStore = useContentManagementStore();

const refresh = () => {
    contentManagementStore.getBoards();
}

const openCreateBoardModal = () => {
    var createBoardModal =  document.getElementById("CreateBoardModal");
    var modal = new Modal(createBoardModal);
    modal.show();
}

const deleteBoard = (boardId: string) => {
    if (boardId.length == 0 || boardId == null) {
        toast.error("Board Id can't be null or empty");
        return;
    }
    contentManagementStore.deleteBoard(boardId);
}

watch(() => contentManagementStore.result_deleteBoard, (newValue) => {
    if(newValue) {
        toast.success("Board deleted successfully");
        contentManagementStore.getBoards();
    }
});

</script>

<template>
    <div class="card card-custom">
        <div class="card-header">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    Boards
                </span>
            </h3>
            <div class="card-toolbar">
                <!--Todo: add search function-->
                <button class="btn btn-primary btn-sm font-weight-bold" style="margin-inline: 10px" @click="openCreateBoardModal()"><i class="fas fa-plus"></i>Create Board</button>
                <ButtonWithLoadingIndicator style="margin-inline: 10px" :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body" v-if="!contentManagementStore.loading">
            <table class="table table-hover table-striped table-bordered" v-if="contentManagementStore.boards.rows.length > 0">
                <thead>
                    <tr>
                        <th>Board Name</th>   
                        <th>Description</th>   
                        <!-- <th>Category</th>    -->
                        <th>Created By</th>   
                        <th>Actions</th>   
                    </tr>
                </thead>
                    <tbody>
                    <tr v-for="element in contentManagementStore.boards.rows">
                        <td> {{ element.board.boardName ?? "N/A" }} </td>
                        <td> {{ element.board.boardDescription ?? "N/A" }}</td>
                        <td> {{ element.createdByUser.user.username ?? "N/A" }}</td>
                        <td> 
                            <button style="margin-inline: 10px" class ="btn btn-sm btn-primary"><i class="fas fa-edit"></i>Edit</button>
                            <button style="margin-inline: 10px" class ="btn btn-sm btn-primary"><i class="fas fa-eye"></i>View</button>
                            <button style="margin-inline: 10px" class="btn btn-sm btn-danger" @click = deleteBoard(element.board.boardId)><i class="fas fa-xmark"></i>Delete</button>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div v-else>
                <p>No boards found</p>
            </div>
        </div>
        <div class="card-body" v-else>
            <LoadingIndicator :loading="true" />
        </div>
    </div>
<CreateBoardModal />
</template>