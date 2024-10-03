<script lang="ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import { ref, watch, type PropType } from 'vue';
import { useAdminStore } from '@/stores/AdminStore';

const selectedUser = ref<UserBasicInfo|null>();
const adminStore = useAdminStore();

watch(() => adminStore.selectedUser, (newSelectedUser) => {
    selectedUser.value = newSelectedUser;
});

const userFirstname = ref<string>("");
const userLastname = ref<string>("");

const saveChanges = () => {
    if (selectedUser.value) {
        //Update the user's data with the new values coming in or leave them if no new ones have been provided.

        if(userFirstname.value) {
            selectedUser.value.user.userFirstname = userFirstname.value;
        }
        if(userLastname.value) {
            selectedUser.value.user.userLastname = userLastname.value;
        }
        //Execute the update
        adminStore.updateUser(selectedUser.value);
    }
}

const closeModal = () => {
    $("#editSelectedUserModal").modal("hide");
}
</script>

<template>
    <div class="modal fade stackable" id="editSelectedUserModal" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="editSelectedUserModalLabel">Edit user</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"><i class="ki ki-close"></i></button>
                </div>
                <div class= "modal-body">
                    <p>Selected User: {{ selectedUser?.user.username }}</p>
                    <div class="row">
                        <div class="col-md-6">
                            <label class="form-label">First name</label>
                            <input type="text" class="form-control" v-model="userFirstname" :placeholder=selectedUser?.user.userFirstname />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label">Last name</label>
                            <input type="text" class="form-control" v-model="userLastname" :placeholder=selectedUser?.user.userLastname />
                        </div>
                    </div>
                    <p>Address and contact info</p>
                    <div class="row">
                        <div class="col-md-3">
                            <label class="form-label">Address</label>
                            <input type="text" class="form-control" :placeholder=selectedUser?.user.address />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">City/Town</label>
                            <input type="text" class="form-control" :placeholder=selectedUser?.user.cityName />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Postcode</label>
                            <input type="text" class="form-control" :placeholder=selectedUser?.user.postalCode />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Country</label>
                            <input type="text" class="form-control" :placeholder=selectedUser?.user.countryName />
                        </div>
                    </div>
                    <p>Personal details</p>
                    <div class="row">
                        <label class="form-label">Gender</label>
                        <select class="form-control" v-model="selectedUser?.user.gender">
                            <option>Male</option>
                            <option>Female</option>
                            <option>Other</option>
                        </select>
                    </div>                  
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger" data-bs-dismiss="modal" @click="closeModal()">Close</button>
                    <button type="button" class="btn btn-primary" @click="saveChanges()">Save changes</button>
                </div>
            </div>
        </div>
    </div>
</template>