<script lang = "ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import { useAdminStore } from '@/stores/AdminStore';
import { useToast } from 'vue-toastification';
import { onMounted, ref } from 'vue';

const adminStore = useAdminStore();
const toast = useToast();

const tabs = ref([
    {
        id: 1,
        name: 'Users',
    },
    {
        id: 2,
        name: 'Roles',
    },
    {
        id: 3,
        name: 'Logs',
    },
    {
        id: 4,
        name: 'Content management',
    },
]);

onMounted(() => {
    adminStore.getUsers();
    adminStore.getRoles();
});

const updateUserInfo = (selectedUser: UserBasicInfo) => {
    toast.info("Edit selected user - coming soon.")
}

const activeTab = ref(1);
</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Administration</span>
            </h3>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-12">
                    <!--Tabbed navigation: Create the tab bar here: Users, Role permissions, Logs, Content management etc-->
                    <div class="card card-custom">
                        <div class="card-header border-0 pt-7">
                            <v-tabs v-model = "activeTab" background-color="transparent">
                                <v-tab v-for="tab in tabs" :key="tab.id">
                                    {{ tab.name }}
                                </v-tab>
                            </v-tabs>
                        </div>
                        <div class="card-body">
                            <v-tab-item v-if="activeTab===0">
                                <div class= "card card-custom">
                                    <div class= "card-header border-0 pt-7">
                                        <button class="btn btn-primary btn-sm m-1" @click="">Create new user</button>
                                    </div>
                                    <div class= "card-body">
                                        <table class="table table-borderless table-sm">
                                            <thead>
                                                <tr>
                                                    <th>User name</th>
                                                    <th>Email address</th>
                                                    <th>Role</th>
                                                    <th>Actions</th>
                                                    <th></th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody v-for="user in adminStore.users">
                                                <tr>
                                                    <td>{{ user.user.username }}</td>
                                                    <td>{{ user.user.emailAddress }}</td>
                                                    <!--Need to see if the role id on the user matches one of the roles and show only that role name-->
                                                    <td v-if="adminStore.roles.filter((role) => role.role.roleId === user.user.roleId)">{{ adminStore.roles.filter((role) => role.role.roleId === user.user.roleId)[0].role.roleName }}</td>
                                                    <td v-else>None</td>
                                                    <td>
                                                        <button class = "btn btn-sm btn-primary" @click="updateUserInfo(user)">Edit</button>
                                                    </td>
                                                    <td>
                                                        <button class="btn btn-sm btn-primary">Reset password</button>
                                                    </td>
                                                    <td>
                                                        <button class = "btn btn-sm btn-danger">Delete</button>
                                                    </td>
                                                </tr> 
                                            </tbody>                            
                                        </table>
                                    </div>
                                </div>
                            </v-tab-item>
                            <v-tab-item v-if="activeTab===1">
                                <table class="table table-borderless table-sm">
                                    <thead>
                                        <tr>
                                            <th>Role name</th>
                                            <th>Number of users</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody v-for="role in adminStore.roles">
                                        <tr>
                                            <td> {{ role.role.roleName }}</td>
                                            <td> {{ adminStore.users.filter((user) => user.user.roleId === role.role.roleId).length  }} </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </v-tab-item>
                            <v-tab-item v-if="activeTab===2">
                                <p>Manage logs - coming soon...</p>
                            </v-tab-item>
                            <v-tab-item v-if="activeTab===3">
                                <p>Manage content - coming soon...</p>
                            </v-tab-item>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>