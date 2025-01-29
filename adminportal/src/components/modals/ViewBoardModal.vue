<script lang="ts" setup>
import type { BoardFullInfo } from '@/Dto/AdminPortal/BoardInfo';
import type { PropType } from 'vue';
import { Modal } from 'bootstrap';
import { inject, ref, watch } from 'vue';

// 1. Props (existing)
const props = defineProps({
    selectedBoard: {
        type: Object as PropType<BoardFullInfo>,
        required: true,
        default: () => ({} as BoardFullInfo)
    }
});

// 2. Emits
const emit = defineEmits(['update:selectedBoard', 'boardUpdated']);

// 3. Inject (if provided by a parent)
const injectedData = inject('boardData', null);

// 4. Refs and expose
const modalData = ref(null);
const updateModalData = (data: any) => {
    modalData.value = data;
};

// Close modal method
const closeModal = () => {
    const modalElement = document.getElementById('ViewBoardModal');
    if (modalElement) {
        const modal = Modal.getInstance(modalElement) || new Modal(modalElement);
        modal.hide();
    }
};

// Loading state
const isLoading = ref(true);

watch(() => props.selectedBoard, (newValue) => {
    isLoading.value = !newValue?.board;
}, { immediate: true });

// Expose methods/data to parent
defineExpose({
    modalData,
    updateModalData
});
</script>

<template>
    <div id="ViewBoardModal" 
         class="modal fade stackable" 
         tabindex="-1"
         role="dialog" 
         aria-labelledby="staticBackdrop" 
         aria-hidden="true" 
         data-bs-backdrop="static"
         data-bs-keyboard="false">
        <div class="modal-dialog modal-xxl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="staticBackdropLabel">
                        View Board {{ selectedBoard?.board?.boardName ?? "-" }}
                    </h5>
                    <button type="button" class="close" aria-label="Close" @click="closeModal">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button>
                </div>
                <div class="modal-body">
                    <div v-if="isLoading" class="text-center p-4">
                        <div class="spinner-border text-primary" role="status">
                            <span class="sr-only">Loading...</span>
                        </div>
                    </div>
                    <div v-else class="container">
                        <!-- Board Details -->
                        <div class="card mb-4">
                            <div class="card-header">
                                <h6 class="mb-0">Board Details</h6>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <p><strong>Board Name:</strong> {{ selectedBoard?.board?.boardName ?? "N/A" }}</p>
                                        <p><strong>Description:</strong> {{ selectedBoard?.board?.boardDescription ?? "N/A" }}</p>
                                        <p><strong>Category ID:</strong> {{ selectedBoard?.board?.categoryId ?? "N/A" }}</p>
                                    </div>
                                    <div class="col-md-6">
                                        <p><strong>Created By:</strong> {{ selectedBoard?.createdByUser?.user?.username ?? "N/A" }}</p>
                                        <p><strong>Total Topics:</strong> {{ selectedBoard?.totalTopics ?? 0 }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Topics List -->
                        <div class="card">
                            <div class="card-header">
                                <h6 class="mb-0">Topics ({{ selectedBoard?.topics?.length ?? 0 }})</h6>
                            </div>
                            <div class="card-body">
                                <div class="table-responsive" v-if="selectedBoard?.topics?.length">
                                    <table class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Topic Name</th>
                                                <th>Created By</th>
                                                <th>Total Posts</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="element in selectedBoard.topics" :key="element.topic.topicId">
                                                <td>{{ element.topic.topicName }}</td>
                                                <td>{{ element.createdByUser?.user?.username }}</td>
                                                <td>{{ element.totalPosts }}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <p v-else class="text-muted">No topics found in this board.</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger btn-sm" @click="closeModal">Close</button>
                </div>
            </div>
        </div>
    </div>
</template>