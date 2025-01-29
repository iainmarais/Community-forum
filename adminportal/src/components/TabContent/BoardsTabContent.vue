<script lang="ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import CreateBoardModal from '@/components/modals/CreateBoardModal.vue';
import ViewBoardModal from '@/components/modals/ViewBoardModal.vue';
import { Modal } from 'bootstrap';
import { useToast } from 'vue-toastification';
import { ref, watch } from 'vue';
import { debounce } from 'lodash';
import PageSelector from '@/components/elements/Inputs/PageSelector.vue';
import SearchBar from '@/components/elements/Inputs/SearchBar.vue';
import type { BoardFullInfo } from '@/Dto/AdminPortal/BoardInfo';

const searchQuery = ref("");
const selectedBoard = ref<BoardFullInfo|null>(null);

const toast = useToast();

const contentManagementStore = useContentManagementStore();

const refresh = () => {
    contentManagementStore.getBoards();
}

const search = debounce((query: string) => {

//Update the search query from the user's input, then trigger the getUserInfo function in the store to pull an updated list of users.
//Need a way to wait until the user has finished entering their query before executing this function.
    contentManagementStore.searchQuery = query;
    contentManagementStore.getBoards();   
}, 300);

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

const viewBoard = (boardId: string) => {
    //Load the full board info from the selected board Id.
    contentManagementStore.getBoardFullInfo(boardId);

    openShowViewBoardModal();
}

const openShowViewBoardModal = () => {
    const viewBoardModal = document.getElementById("ViewBoardModal");
    if (viewBoardModal) {
        const modal = new Modal(viewBoardModal, {
            backdrop: true,
            keyboard: true,
            focus: true
        });
        modal.show();
    }
}

watch(() => contentManagementStore.selectedBoard, (newValue) => {
    if(newValue) {
        selectedBoard.value = contentManagementStore.selectedBoard;
    }
});

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
                <SearchBar v-model:searchQuery="searchQuery" :handleSearch = "search" />
                <button class="btn btn-primary btn-sm font-weight-bold" style="margin-inline: 10px" @click="openCreateBoardModal()"><i class="fas fa-plus"></i>Create Board</button>
                <ButtonWithLoadingIndicator style="margin-inline: 10px" :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body" v-if="!contentManagementStore.loading">
            <PageSelector :current-page-number="contentManagementStore.currentPageNumber" :totalPages="contentManagementStore.boards.totalPages" :rows-per-page="contentManagementStore.rowsPerPage" @previous-page="$emit('previous-page')" @next-page="$emit('next-page')" />
            <table class="table table-hover table-striped table-bordered" v-if="contentManagementStore.boards.rows.length > 0">
                <thead>
                    <tr>
                        <th>Board Name</th>   
                        <th>Description</th>   
                        <th>Topics</th>
                        <!-- <th>Category</th>    -->
                        <th>Created By</th>   
                        <th>Actions</th>   
                    </tr>
                </thead>
                    <tbody>
                    <tr v-for="element in contentManagementStore.boards.rows">
                        <td> {{ element.board.boardName ?? "N/A" }} </td>
                        <td> {{ element.board.boardDescription ?? "N/A" }}</td>
                        <td> {{ element.numTopics ?? 0 }}</td>
                        <td> {{ element.createdByUser.user.username ?? "N/A" }}</td>
                        <td> 
                            <button style="margin-inline: 10px" class ="btn btn-sm btn-primary"><i class="fas fa-edit"></i>Edit</button>
                            <button style="margin-inline: 10px" class ="btn btn-sm btn-primary" @click="viewBoard(element.board.boardId)"><i class="fas fa-eye"></i>View</button>
                            <button style="margin-inline: 10px" class="btn btn-sm btn-outline-danger" @click = "deleteBoard(element.board.boardId)"><i class="fas fa-xmark"></i>Delete</button>
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
<ViewBoardModal :selected-board = "selectedBoard" />
</template>