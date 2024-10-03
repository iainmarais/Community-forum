<script lang="ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import { ref, watch, type PropType } from 'vue';
import { useAdminStore } from '@/stores/AdminStore';
import CustomSelector from '@/components/elements/CustomSelector.vue';

const selectedUser = ref<UserBasicInfo|null>();
const adminStore = useAdminStore();

watch(() => adminStore.selectedUser, (newSelectedUser) => {
    selectedUser.value = newSelectedUser;
});

const userFirstname = ref<string>("");
const userLastname = ref<string>("");
const userAddress = ref<string>("");
const userCityName = ref<string>("");
const userCountryName = ref<string>("");
const userPostalCode = ref<string>("");
const userGender = ref<string>("");


const saveChanges = () => {
    if (selectedUser.value) {
        //Update the user's data with the new values coming in or leave them if no new ones have been provided.

        if(userFirstname.value) {
            selectedUser.value.user.userFirstname = userFirstname.value;
        }
        if(userLastname.value) {
            selectedUser.value.user.userLastname = userLastname.value;
        }
        if(userAddress.value) {
            selectedUser.value.user.address = userAddress.value;
        }
        if(userCityName.value) {
            selectedUser.value.user.cityName = userCityName.value;
        }
        if(userCountryName.value) {
            selectedUser.value.user.countryName = userCountryName.value;
        }
        if(userPostalCode.value) {
            selectedUser.value.user.postalCode = userPostalCode.value;
        }
        if(userGender.value) {
            selectedUser.value.user.gender = userGender.value;
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
                            <input type="text" class="form-control" v-model="userAddress" :placeholder=selectedUser?.user.address />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">City/Town</label>
                            <input type="text" class="form-control" v-model="userCityName" :placeholder=selectedUser?.user.cityName />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Postcode</label>
                            <input type="text" class="form-control" v-model="userPostalCode" :placeholder=selectedUser?.user.postalCode />
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Country</label>
                            <input type="text" class="form-control" v-model="userCountryName" :placeholder=selectedUser?.user.countryName />
                        </div>
                    </div>
                    <p>Personal details</p>
                    <div class="row">
                        <label class="form-label">Gender</label>
                        <CustomSelector 
                            :style-class="'form-control'"
                            :options="[{value: 'Male', displayText: 'Male'}, {value: 'Female', displayText: 'Female'}, {value: 'Other', displayText: 'Other'}]" 
                            :selected-value="selectedUser?.user.gender!"
                            v-model="userGender" />
                        <!-- <select class="form-control" v-model="userGender" :placeholder=selectedUser?.user.gender>
                            <option>Male</option>
                            <option>Female</option>
                            <option>Other</option>
                        </select> -->
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