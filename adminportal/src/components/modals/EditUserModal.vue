<script lang = "ts" setup> 
import type { UserEntry } from '@/Dto/AdminPortal/UserInfo';
import { ref, type PropType } from 'vue';
const props = defineProps({
    selectedUser: {
        type: Object as PropType<UserEntry>
    }
})

const userFirstname = ref<string>(props.selectedUser.userFirstname);
const userLastname = ref<string>(props.selectedUser?.userLastname);

const saveChanges = () => {
    //Todo: build out. This will execute an endpoint against the server
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
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger btn-sm" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary btn-sm" @click="saveChanges()">Save changes</button>
                </div>
            </div>
        </div>
    </div>
</template>