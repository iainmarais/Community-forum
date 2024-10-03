<script lang = "ts" setup>
import type { CreateUserRequest } from '@/Dto/UserInfo';
import { useAdminStore } from '@/stores/AdminStore';
import { ref } from 'vue';
import { useToast } from 'vue-toastification';

const adminStore = useAdminStore();
const toast = useToast();

const username = ref<string>("");
const emailAddress = ref<string>("");
const password = ref<string>("");
const retypePassword = ref<string>("");
const roleName = ref<string>("");

const closeModal = () => {
    // @ts-ignore
    $("#createUserModal").modal("hide");
};

const createUser = () => {
    if (password.value !== retypePassword.value) {
        toast.error("Passwords do not match");
        return;
    }

    if (!username.value || !emailAddress.value || !password.value || !retypePassword.value || !roleName.value) {
        toast.error("Please fill in all required fields");
        return;
    }
    const createUserRequest: CreateUserRequest = {
        username: username.value,
        emailAddress: emailAddress.value,
        password: password.value,
        retypePassword: retypePassword.value,
        roleName: roleName.value
    }
    adminStore.createUser(createUserRequest);
}


</script>

<template>
    <div id ="createUserModal" class="modal fade stackable" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <!--note to self: modal-dialog, not modal dialog-->
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Create New User</h5>
                    <button type="button" class="close" @click="closeModal()" aria-label="Close">
                        <i aria-hidden="true" class="ki ki-close"></i>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="username">Username</label>
                        <input type="text" class="form-control" id="username" v-model="username">
                    </div>
                    <div class="form-group">
                        <label for="emailAddress">Email Address</label>
                        <input type="text" class="form-control" id="emailAddress" v-model="emailAddress">
                    </div>
                    <div class="form-group">
                        <label for="password">Password</label>
                        <input type="password" class="form-control" id="password" v-model="password">
                    </div>
                    <div class="form-group">
                        <label for="retypePassword">Retype Password</label>
                        <input type="password" class="form-control" id="retypePassword" v-model="retypePassword">
                    </div>
                    <div class="form-group">
                        <label for="roleName">Role Name</label>
                        <select class="form-control" id="roleName" v-model="roleName">
                            <option v-for="role in adminStore.roles">{{ role.role.roleName }}</option>
                        </select>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary font-weight-bold" @click="createUser()">Create</button>
                    <button type="button" class="btn btn-outline-danger font-weight-bold" @click="closeModal()">Close</button>
                </div>
            </div>
        </div>
    </div>
</template>