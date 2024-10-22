<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import LoadingIndicator from '@/components/LoadingIndicator.vue';

import { useUserManagementStore } from '@/stores/UserManagementStore';
import { onMounted } from 'vue';
import dayjs from 'dayjs';

const userManagementStore = useUserManagementStore();

const refresh = () => {
    userManagementStore.getUserInfo();
}

const formatDate = (date: Date) => {
    return dayjs(date).format('DD/MM/YYYY HH:mm:ss');
}

onMounted(() => {
    userManagementStore.getUserInfo();
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
                <ButtonWithLoadingIndicator :label="'Refresh'" :icon="'fas fa-sync'"  class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="text-center">
                        <div v-if="!userManagementStore.loading_getUserInfo">
                            <div v-if="userManagementStore.result_getUserInfoSuccess">
                                <table class="table table-hover table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th class="text-center">Username</th>
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
                                            <td class="text-center">{{ user.user.emailAddress }}</td>
                                            <td class="text-center">{{  formatDate(user.user.registrationTime) }}</td>
                                            <td class="text-center">{{  formatDate(user.user.lastLoginTime) }}</td>
                                            <td class="text-center">{{ user.role.roleName ?? 'User' }}</td>
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
</template>