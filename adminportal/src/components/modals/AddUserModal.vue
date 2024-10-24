<script lang = "ts" setup>
import type { AddUserRequest } from '@/Dto/AdminPortal/AddUserRequest';
import type { RoleEntry } from '@/Dto/AdminPortal/RoleInfo';
import { useUserManagementStore } from '@/stores/UserManagementStore';
import { onMounted, ref, watch } from 'vue';
import { useToast } from 'vue-toastification';
import Select2 from 'vue3-select2-component';
const userManagementStore = useUserManagementStore();

const toast = useToast();

const newUsername = ref("");
const newPassword = ref("");
const newEmailAddress = ref("");
const newRoleId = ref("");

const availableRoles = ref([]);
const selectedRole = ref<string>("")

const getRoles = async () => {
    await userManagementStore.getUserRoles();
    availableRoles.value = userManagementStore.userRoles.map((role: RoleEntry) => ({
        id: role.roleType, 
        text: role.roleName,
    }));
}

onMounted(() => {
    getRoles();
});

watch(() => userManagementStore.userRoles,(newValue) => {
    if(newValue) {
        availableRoles.value = newValue.map((role: RoleEntry)=> ({
            id: role.roleType, 
            text: role.roleName,
        }));
    }
});


const addUser = () => {

    if(newUsername.value == "" || newPassword.value == "" || newEmailAddress.value == "") {
        toast.error("Please enter a username, password, and email address.");
        //Stop processing here if any of these are not provided.
        return;
    }
    if(newRoleId.value == "") {
        toast.info("Role Id not provided, assuming an ordinary user.");
        newRoleId.value = "User";
    }

    const req: AddUserRequest = {
        username: newUsername.value,
        password: newPassword.value,
        emailAddress: newEmailAddress.value,
        roleId: newRoleId.value
    }

    //Send the request to the store, which will pass it through to the service.
    userManagementStore.addUser(req);
}

</script>

<template>
    <div id="addUserModal" class="modal fade stackable" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-xxl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title">
                        <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                            Add a new user
                        </span>
                    </h3>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>Username</label>
                        <input v-model="newUsername" type="text" class="form-control" placeholder="Username">
                    </div>
                    <div class="form-group">
                        <label>Password</label>
                        <input v-model="newPassword" type="text" class="form-control" placeholder="Password">
                    </div>
                    <div class="form-group">
                        <label>Email Address</label>
                        <input v-model="newEmailAddress" type="email" class="form-control" placeholder="Email Address">
                    </div>
                    <div class="form-group">
                        <label>Role</label>
                        <Select2 v-model="newRoleId" :options="availableRoles" class="wider-select2" style="width: 60%" placeholder = "Select a role to assign" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger btn-sm" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary btn-sm" @click="addUser()">Add user</button>
                </div>
            </div>
        </div>
    </div>
</template>