<script lang="ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import { debounce } from 'lodash';
import { useUserManagementStore } from '@/stores/UserManagementStore';
import { onMounted, ref, watch } from 'vue';
import dayjs from 'dayjs';
import PageSelector from '../elements/Inputs/PageSelector.vue';
import SearchBar from '../elements/Inputs/SearchBar.vue';
import AddUserModal from '../modals/AddUserModal.vue';
import EditUserModal from '../modals/EditUserModal.vue';
import { Modal } from 'bootstrap';
import type { UserEntry } from '@/Dto/AdminPortal/UserInfo';
import { useToast } from 'vue-toastification';
import { useAppContextStore } from '@/stores/AppContextStore';
import type { AssignRoleRequest } from '@/Dto/AdminPortal/AssignRoleRequest';
import type { RoleEntry } from '@/Dto/AdminPortal/RoleInfo';
import Swal from 'sweetalert2';
import SwalConfirm from '@/components/SwalConfirmations/SwalConfirm';

const toast = useToast();

const appContextStore = useAppContextStore();
const userManagementStore = useUserManagementStore();
const searchQuery = ref("");

const selectedRoleName = ref<string>("");
const selectedRoleId = ref<string>("");
const userToEdit = ref<UserEntry|null>(null);

const refresh = () => {
    userManagementStore.getUserInfo();
}

const formatDate = (date: Date) => {
    return dayjs(date).format('DD/MM/YYYY HH:mm:ss');
}

const openEditUserModal = (selectedUser: UserEntry) => {
    userToEdit.value = selectedUser;
    const editUserModal = document.getElementById("editUserModal");
    if (editUserModal) {
        const modal = new Modal(editUserModal, {
            backdrop: true,
            keyboard: true,
            focus: true
        });
        modal.show();
    }
}

const openAddUserModal = () => {
    var addUserModal = document.getElementById("addUserModal");
    var modal = new Modal(addUserModal);
    modal.show();
}
const banUser = (user: UserEntry) => {
    if(user.roleId == "Admin" && user.userId==appContextStore.currentLoggedInUser.userId) {
        toast.error("You can't ban yourself.");
        return;
    }
    if(user.roleId == "Admin") {
        toast.error("This action is not supported.");
        return;
    }
}
const deleteUser = (userId: string) => {
    userManagementStore.deleteUser(userId);
}

//Test: show sweetalert confirmation from custom component
const assignRoleToUser = (selectedUser: UserEntry) => {

    userManagementStore.getUserRoles();
    const roles = userManagementStore.userRoles.map((role: RoleEntry) => {
        return `<option value="${role.roleType}">${role.roleName}</option>`;
    })
    //This works nicely... but how do I get it into its own component that can be reused? <scratch-head />
    Swal.fire({
        title: `Assign role to ${selectedUser.username}`,
        text: "Select a role from the dropdown below to assign to the selected user.",
        html: ` 
            <select id= "roleSelect">
                ${roles.join("")}
            </select>
            `,
        showCancelButton: true,
        confirmButtonText: 'Assign Role',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        preConfirm: () => {
            const selectedRole = (document.getElementById("roleSelect") as HTMLSelectElement).value;
            return selectedRole;
        }
    }).then((result) => {
        if (result.isConfirmed) {
            const selectedRoleType = result.value;  // Captured from preConfirm
            const selectedRoleName = userManagementStore.userRoles.find(role => role.roleType === selectedRoleType)?.roleName;
            if(!selectedRoleName) {
                toast.error("No role selected or role not found.");
                return;
            }
            const assignRoleRequest: AssignRoleRequest = {
                selectedUserId: selectedUser.userId,
                selectedRoleId: selectedRoleType,
                selectedRoleName: selectedRoleName
            
            };
            if(assignRoleRequest.selectedUserId == appContextStore.currentLoggedInUser.userId) {
                toast.error("You can't assign yourself a role.");
                return;
            }

            if(assignRoleRequest.selectedRoleId == "Admin" && assignRoleRequest.selectedUserId==appContextStore.currentLoggedInUser.userId) {
                toast.error("You can't assign yourself a role.");
                return;
            }
            userManagementStore.assignUserRole(selectedUser.userId, assignRoleRequest);
            userManagementStore.getUserInfo();
        }
    })
}

const search = debounce((query: string) => {

    //Update the search query from the user's input, then trigger the getUserInfo function in the store to pull an updated list of users.
    //Need a way to wait until the user has finished entering their query before executing this function.
        userManagementStore.searchQuery = query;
        userManagementStore.getUserInfo();   
    }, 300);

onMounted(() => {
    userManagementStore.getUserInfo();
});

watch(() => userManagementStore.result_assignUserRoleSuccess,(newValue) => {
    if(newValue) {
        userManagementStore.getUserInfo();
    }
})

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    User management
                </span>
            </h3>
            <div class="card-toolbar">
                <SearchBar v-model:searchQuery="searchQuery" :handleSearch = "search" />
                <ButtonWithLoadingIndicator :label="'Refresh'" :icon="'fas fa-sync'"  class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
                <button class="btn btn-sm btn-primary" @click="openAddUserModal()"><i class="fas fa-plus"></i>Add User</button>
            </div>
        </div>
        <div class="card-body">
            <!--Page selector-->
            <PageSelector :current-page-number="userManagementStore.currentPageNumber" :totalPages="userManagementStore.users.totalPages" :rows-per-page="userManagementStore.rowsPerPage" @previous-page="$emit('previous-page')" @next-page="$emit('next-page')" />
            <div class="row">
                <div class="col-md-12">
                    <div class="text-center">
                        <div v-if="!userManagementStore.loading_getUserInfo">
                            <div v-if="userManagementStore.result_getUserInfoSuccess">
                                <table class="table table-hover table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th class="text-center">Username</th>
                                            <th class="text-center">First name</th>
                                            <th class="text-center">Surname</th>
                                            <th class="text-center">Email address</th>
                                            <th class="text-center">Date registered</th>
                                            <th class="text-center">Date last logged in</th>
                                            <th class="text-center">Role</th>
                                            <th class="text-center">Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="user in userManagementStore.users.rows">
                                            <td class="text-center">{{ user.user.username }}</td>
                                            <td class="text-center">{{ user.user.userFirstname ?? "" }} </td>
                                            <td class="text-center">{{ user.user.userLastname ?? "" }} </td>
                                            <td class="text-center">{{ user.user.emailAddress }}</td>
                                            <td class="text-center">{{ formatDate(user.user.registrationTime) }}</td>
                                            <td class="text-center">{{ formatDate(user.user.lastLoginTime) }}</td>
                                            <td class="text-center">{{ user.role.roleName ?? 'User' }}</td>
                                            <td>
                                                <!--Space these out by around 10 px-->
                                                <button style="margin-inline: 10px" class ="btn btn-sm btn-primary" @click="openEditUserModal(user.user)"><i class="fas fa-edit"></i>Edit</button>
                                                <button style="margin-inline: 10px" class ="btn btn-sm btn-primary" @click="assignRoleToUser(user.user)"><i class="fas fa-check"></i>Assign role</button>
                                                <button style="margin-inline: 10px" class="btn btn-sm btn-danger" @click="banUser(user.user)"><i class="fas fa-ban"></i>Ban</button>
                                                <button style="margin-inline: 10px" class="btn btn-sm btn-danger" @click="deleteUser(user.user.userId)"><i class="fas fa-xmark"></i>Delete</button>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>  
                        <LoadingIndicator v-else :loading="true" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <AddUserModal />
    <EditUserModal 
        v-if="userToEdit"
        :selected-user="userToEdit" 
    />
</template>