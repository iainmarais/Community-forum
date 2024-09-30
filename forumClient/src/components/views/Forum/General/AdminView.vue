<script lang = "ts" setup>
import { useAdminStore } from '@/stores/AdminStore';
import { useToast } from 'vue-toastification';
import { onMounted, ref } from 'vue';

const adminStore = useAdminStore();
const toast = useToast();

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
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Administration</span>
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
                            <table class="table table-borderless table-sm">
                                <thead>
                                    <tr>
                                        <th>User</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="element in adminStore.users">
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
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
                </div>
            </div>
        </div>
    </div>
</template>