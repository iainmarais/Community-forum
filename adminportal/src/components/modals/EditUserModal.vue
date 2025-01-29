<script lang="ts" setup>
import type { UpdateUserRequest } from '@/Dto/AdminPortal/UpdateUserRequest';
import type { UserEntry } from '@/Dto/AdminPortal/UserInfo';
import { useUserManagementStore } from '@/stores/UserManagementStore';
import { Modal } from 'bootstrap';
import { ref, watch, onMounted, type PropType } from 'vue';
import { useToast } from 'vue-toastification';

const toast = useToast();
const userManagementStore = useUserManagementStore();

const props = defineProps({
    selectedUser: {
        type: Object as PropType<UserEntry>,
        required: true,
        default: () => ({} as UserEntry)
    }
})

const userFirstname = ref('');
const userLastname = ref('');
const userEmailAddress = ref('');

onMounted(() => {
    userFirstname.value = props.selectedUser?.userFirstname || '';
    userLastname.value = props.selectedUser?.userLastname || '';
    userEmailAddress.value = props.selectedUser?.emailAddress || '';
});

watch(() => props.selectedUser, (newUser) => {
    if (newUser) {
        userFirstname.value = newUser.userFirstname || '';
        userLastname.value = newUser.userLastname || '';
        userEmailAddress.value = newUser.emailAddress || '';
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
    //Need this to query the db and find the appropriate user to update.
    userUpdateRequest.userId = props.selectedUser.userId;
    //Check for each of the fields to propagate to the request
    //For each prop set, use that value else use the existing value.
    if(userFirstname.value) {
        userUpdateRequest.userFirstname = userFirstname.value;
    } else {
        userUpdateRequest.userFirstname = props.selectedUser.userFirstname;
    }

    if(userLastname.value) {
        userUpdateRequest.userLastname = userLastname.value;
    } else {
        userUpdateRequest.userLastname = props.selectedUser.userLastname;
    }

    //This one is a bit of a tricky situation since the email address is also used for logging in, 
    //however this is the admin portal, and administrators should be able to update email addresses and usernames.
    if(userEmailAddress.value) {
        userUpdateRequest.emailAddress = userEmailAddress.value;
    } else {
        userUpdateRequest.emailAddress = props.selectedUser.emailAddress;
    }
    //Todo: add username update code to this method.
    userManagementStore.updateUser(props.selectedUser?.userId, userUpdateRequest);
}

</script>

<template>
    <div id="editUserModal" 
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
                        <textarea 
                            class="form-control" 
                            id="UserFirstnameInput" 
                            v-model="userFirstname" 
                            :placeholder="props.selectedUser?.userFirstname || 'Enter first name'">
                        </textarea>
                    </div>
                    <div class="form-group">
                        <label>Last name</label>
                        <textarea 
                            class="form-control" 
                            id="UserLastnameInput" 
                            v-model="userLastname" 
                            :placeholder="props.selectedUser?.userLastname || 'Enter last name'">
                        </textarea>
                    </div>
                    <div class="form-group">
                        <label>Email address</label>
                        <textarea 
                            class="form-control" 
                            id="UserEmailAddress" 
                            v-model="userEmailAddress" 
                            :placeholder="props.selectedUser?.emailAddress || 'Enter email address'">
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