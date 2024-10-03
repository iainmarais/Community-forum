<script lang = "ts" setup>
import { useAdminStore } from '@/stores/AdminStore';
import { useToast } from 'vue-toastification';
import { onMounted, ref, watch } from 'vue';
import { createHubConnection, initConnection, sendSignalRMessage } from '@/components/utils/SignalRHelper';
import { type UserBasicInfo } from '@/Dto/UserInfo';

import CreateNewUserModal from '@/components/modals/Admin/CreateNewUserModal.vue';
import EditSelectedUserModal from '@/components/modals/Admin/EditSelectedUserModal.vue';

const adminStore = useAdminStore();
const toast = useToast();

const hubs: Record<string, string> = {
    "Chat": "chat",
    "Forum stats": "forumStats"
}

const selectedUser = ref<UserBasicInfo>({} as UserBasicInfo);

const selectedHub = ref<string>("");
const hubMethodName = ref<string>("");
const args = ref<string>("");
const clientMethodName = ref<string>("");


//User management
const openCreateUserModal = () => {
    $("#createUserModal").modal("show");
}


const editSelectedUser = () => {
    //If selected user is null, toast error message and return.
    if(!selectedUser.value) {
        toast.error("No user selected.");
        return;
    }
    console.log(selectedUser.value);
    adminStore.selectedUser = selectedUser.value;
    $("#editSelectedUserModal").modal("show");
}

const deleteSelectedUser = () => {
    if(!selectedUser.value) {
        toast.error("No user selected.");
        return;
    }
}

//SignalR testing and development.
const connectToHub = (hubName: string, clientMethodName: string) => {
    createHubConnection(hubName);
    initConnection(hubName, clientMethodName);
}

const sendMessage = (hubMethodName: string, args: string) => {  
    sendSignalRMessage(hubMethodName, args);
}

const tabs = ref([
    {
        id: 1,
        name: 'Users',
        iconClass: 'fas fa-user',
    },
    {
        id: 2,
        name: 'Roles',
        iconClass: 'fas fa-lock',
    },
    {
        id: 3,
        name: 'Logs',
        iconClass: 'fas fa-file-alt',
    },
    {
        id: 4,
        name: 'Content management',
        iconClass: 'fas fa-wrench',
    },
    {
        id: 5,
        name: "Testing and development",
        iconClass: 'fas fa-gear',
    }
]);

onMounted(() => {
    adminStore.getUsers();
    adminStore.getRoles();
});

const setActiveTab = (tabId: number) => {
    activeTab.value = tabId;
}

const activeTab = ref(1);

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Administration and Management</span>
            </h3>
        </div>
        <div class="card-body">
            <ul class="nav nav-tabs" id="myTab" role="tablist">
                <li class="nav-item" v-for="tab in tabs" :key="tab.id">
                    <a class="nav-link" :id="`#tab-${tab.id}-tab`" data-toggle="tab" :href="`#tab-${tab.id}`" role="tab" :aria-controls="`tab-${tab.id}`" aria-selected="true" @click="setActiveTab(tab.id)">
                        <i :class= tab.iconClass></i> {{ tab.name }}
                    </a>
                </li>
            </ul>
            <div class="tab-content" id="myTabContent">
                <div class="tab-pane container fade" v-for="tab in tabs" :key="tab.id" :id="`tab-${tab.id}`" role="tabpanel" :aria-labelledby="`tab-${tab.id}-tab`" v-show="activeTab === tab.id">
                    <div v-if="activeTab === 1">
                        <!--User management functions will go here.-->
                        <div v-if="adminStore.users">
                            <div class="card card-custom">
                                <div class="card-header border-0 pt-7">
                                    <h3 class="card-title align-items-start flex-column">
                                        <span class = "card-label font-weight-bolder text-dark075 font-size-h5">Manage users</span>
                                    </h3>
                                    <div class="card-toolbar" style="display:flex; gap: 10px;">
                                        <!--toolbar-->
                                        <button class="btn-sm btn-primary" @click="openCreateUserModal">
                                            <i class="fas fa-plus"></i>
                                            Create new
                                        </button>
                                        <button class ="btn-sm btn-primary" @click="editSelectedUser">
                                            <i class="fas fa-edit"></i>
                                            Edit selected
                                        </button>
                                        <button class="btn-sm btn-outline-danger" @click="deleteSelectedUser">
                                            <i class="fas fa-trash"></i>
                                            Delete selected
                                        </button>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <table class="table table-borderless table-sm">
                                            <thead>
                                                <tr>
                                                    <th></th>
                                                    <th>User name</th>
                                                    <th>Email address</th>
                                                    <th>Role</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr v-for="element in adminStore.users" :key="element.user.userId">
                                                    <td>

                                                        <input type="radio" class="form-check-input" name="selectedUser" v-model="selectedUser" :value="element" />
                                                    </td>
                                                    <td>{{ element.user.username }}</td>
                                                    <td>{{ element.user.emailAddress }}</td>
                                                    <!--Identify the role from the roles on the database using the user roleId-->
                                                    <td>{{ adminStore.roles.filter(role => role.role.roleId === element.user.roleId)[0].role.roleName }}</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div v-if="activeTab === 2">
                        <!--Role management functions will go here.-->
                        <div v-if="adminStore.roles">
                            <table class="table table-borderless table-sm">
                                <thead>
                                    <tr>
                                        <th>Role</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="element in adminStore.roles">
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div v-if="activeTab === 3">
                    </div>
                    <div v-if="activeTab === 4">
                    </div>
                    <div v-if="activeTab === 5">
                        <!--Testing and development functions will go here, including for testing SignalR communication.-->
                        <div class= "card card-custom">
                            <div class="card-header border-0 pt-7">
                                <h3 class="card-title align-items-start flex-column">
                                    <span class="card-label font-weight-bolder text-dark075 font-size-h5">Testing and development</span>
                                </h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <!--Form group--> 
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>SignalR Hubs</label>
                                            <div class="form-row align-items-center">
                                                <div class="col-md-3">
                                                    <select class="form-control" v-model="selectedHub">
                                                        <option v-for="hub in hubs">{{ hub }}</option>
                                                    </select>
                                                </div>
                                                <div class="col-md-3">
                                                    <input placeholder = "Client method name" type="text" class="form-control" v-model="clientMethodName" />
                                                </div>
                                                <div class="col-md-1">
                                                    <button class="btn btn-primary w-100" @click="connectToHub(selectedHub, clientMethodName)">Connect</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Test connection: send method name and JSON message</label>
                                            <div class="form-row align-items-center">
                                                <div class="col-md-3">
                                                    <input type="text" class="form-control" v-model="hubMethodName"  placeholder="Hub method name" >
                                                </div>
                                                <div class="col-md-3">
                                                    <input type="text" class="form-control" v-model="args" placeholder="Arguments">
                                                </div>
                                                <div class="col-md-1">
                                                    <button class="btn btn-primary w-100" @click="sendMessage(hubMethodName, args)">Test</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <!--Output from the SignalR server-->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <CreateNewUserModal />
    <EditSelectedUserModal />
</template>