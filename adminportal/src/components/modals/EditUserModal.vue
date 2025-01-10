<script lang = "ts" setup> 
import type { UpdateUserRequest } from '@/Dto/AdminPortal/UpdateUserRequest';
import type { UserEntry } from '@/Dto/AdminPortal/UserInfo';
import { useUserManagementStore } from '@/stores/UserManagementStore';
import { Modal } from 'bootstrap';
import { ref, watch, type PropType } from 'vue';
import { useToast } from 'vue-toastification';

const toast = useToast();
const userManagementStore = useUserManagementStore();

const props = defineProps({
    selectedUser: {
        type: Object as PropType<UserEntry>,
        required: true
    }
})

const userFirstname = ref(props.selectedUser.userFirstname);
const userLastname = ref(props.selectedUser?.userLastname);
const userEmailAddress = ref(props.selectedUser?.emailAddress);

watch(() => props.selectedUser, (newUser) => {
    if (newUser) {
        userFirstname.value = newUser.userFirstname;
        userLastname.value = newUser.userLastname;
        userEmailAddress.value = newUser.emailAddress;
    }
});

watch(() => userManagementStore.result_updateUserSuccess, (newValue) => {
    if(newValue) {
        //Close the modal, notify the user the update was successful
        toast.success(`User ${props.selectedUser?.username} updated successfully`);
        closeModal();
    }
});


const closeModal = () => {
    var modal = document.getElementById("EditUserModal");
    var modalInstance = Modal.getInstance(modal);
    modalInstance?.hide();
}

const saveChanges = () => {
    const userUpdateRequest = {} as UpdateUserRequest;
    if(userFirstname || userLastname) {
        userUpdateRequest.userFirstname = userFirstname.value;
        userUpdateRequest.userLastname = userLastname.value;
    }
    userManagementStore.updateUser(props.selectedUser?.userId, userUpdateRequest);
}

</script>

<template>
    <div id="editUserModal" class="modal fade stackable" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-xxl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title">
                        <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                            Edit user {{ props.selectedUser.username }}
                        </span>
                    </h3>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>First name</label>
                        <textarea class="form-control" id="UserFirstnameInput" :v-model="userFirstname">
                        </textarea>
                    </div>
                    <div class="form-group">
                        <label>Last name</label>
                        <textarea class="form-control" id = "UserLastnameInput" :v-model="userLastname">
                        </textarea>
                    </div>
                    <div class="form-group">
                        <label>Email address</label>
                        <textarea class="form-control" id = "UserEmailAddress" :v-model="userEmailAddress">
                        </textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger btn-sm" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary btn-sm" @click="saveChanges()">Save changes</button>
                </div>
            </div>
        </div>
    </div>
</template>