<script lang = "ts" setup>

import { onMounted, computed, ref, watch, type PropType } from 'vue';
import TabItem from './TabItem.vue';
import type { UserBasicInfo } from '@/Dto/UserInfo';

const tabs = [
    {tabId: 'usermgmt', tabName: "User management", tabIcon: "fas fa-user" },
    {tabId: 'rolemgmt', tabName: "Role management", tabIcon: "fas fa-lock" },
    {tabId: 'contentmgmt', tabName: "Content management", tabIcon: "fas fa-wrench" }, //This might have to be a tabbed navigation component as well: Forum categories, boards, threads, posts can be selected/managed here.
    {tabId: 'logs', tabName: "Logs", tabIcon: "fas fa-file-alt" },
]

const props = defineProps({
    incomingData: {
        type: Object as PropType<any>,
        required: true
    }
})

const isUserInfoArray = computed(() => isUserBasicInfoArray(props.incomingData));

//Type guards for handling tab content since I am passing it in as an array of objects.
function isUserBasicInfoArray(incomingData: any): incomingData is UserBasicInfo[] {
    return Array.isArray(incomingData) && incomingData.every(element => 
        typeof element.user === 'object' &&
        typeof element.user.userId === 'string' &&
        typeof element.user.userFirstname === 'string' &&
        typeof element.user.userLastname === 'string' &&
        typeof element.user.userProfileImageBase64 === 'string' &&
        typeof element.user.emailAddress === 'string' &&
        typeof element.user.roleId === 'string' &&
        typeof element.userFullName === 'string'
    );
}

const activeTabId = ref<string>(tabs[0].tabId);

const setActiveTab = (tabId: string) => {
    activeTabId.value = tabId;
}

</script>

<template>
    <ul class="nav nav-tabs" id="myTab" role="tablist">
        <TabItem v-for="tab in tabs" :key="tab.tabId" :tabId="tab.tabId" :tabName="tab.tabName" :tabIcon="tab.tabIcon" :isActive="activeTabId === tab.tabId" @tabSelected="setActiveTab(tab.tabId)"/>
    </ul>
    <div class = "tab-content" id="myTabContent">
        <div class = "tab-pane container fade" :class="{'show active': activeTabId === tab.tabId}" v-for="tab in tabs" :key="tab.tabId">
            <div v-if="activeTabId === 'usermgmt'">
                <!--User management functions will go here.-->
                <div class="card" v-if="isUserInfoArray">
                    <table class="table table-borderless table-sm">
                        <thead>
                            <tr>
                                <th>User</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="element in props.incomingData">
                                <td></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div> 
            <div v-else-if="activeTabId === 'rolemgmt'">
            </div> 
            <div v-else-if="activeTabId === 'contentmgmt'">
            </div> 
            <div v-else-if="activeTabId === 'logs'">
            </div>
        </div>
    </div>  
</template>