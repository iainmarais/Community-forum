<script lang = "ts" setup>

import { ref, type PropType } from 'vue';
import TabItem from './TabItem.vue';
import TabContentFrame from './TabContentFrame.vue';

const tabs = [
    {tabId: 'users', tabName: "Users", tabIcon: "fas fa-user" },
]

const activeTab = ref(tabs[0].tabId);

const setActiveTab = (tabId: string) => {
    activeTab.value = tabId;
}

</script>

<template>
    <ul class="nav nav-tabs" id="myTab" role="tablist">
        <TabItem v-for="tab in tabs" :key="tab.tabId" :tabId="tab.tabId" :tabName="tab.tabName" :tabIcon="tab.tabIcon" :isActive="activeTab === tab.tabId" @click="setActiveTab(tab.tabId)" />
    </ul>
    <div class = "tab-content" id="myTabContent">
        <TabContentFrame v-for="tab in tabs" :key="tab.tabId" :tabId="tab.tabId" :activeTabId="activeTab">
            <template v-if="tab.tabId==='users'">
                <h3>User Management</h3>
                <p>Coming soon...</p>
            </template>
        </TabContentFrame>
    </div>  
</template>