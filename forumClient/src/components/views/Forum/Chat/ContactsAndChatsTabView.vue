<script lang = "ts" setup>
import type { ChatMessageBasicInfo } from '@/Dto/app/ChatMessageInfo';
import type { ContactBasicInfo } from '@/Dto/app/ContactInfo';
import UserService from '@/services/UserService';
import { onMounted, ref, type PropType } from 'vue';
import { useToast } from 'vue-toastification';

const toast = useToast();


//Grab the contacts and chats from the parent component as props:
const props = defineProps({
    contacts: Object as PropType<ContactBasicInfo[]>,
    chats: Object as PropType<ChatMessageBasicInfo[]>
});

//Send events to the parent for selection of chat and/or contact:
const emit = defineEmits<{
    (event: "chatSelected", chat: ChatMessageBasicInfo): void,
    (event: "contactSelected", contact: ContactBasicInfo): void
}>();

const getUserInfoById = async (userId: string) => {
    var response = await UserService.getUserById(userId);
    if (!response) {
        toast.error("Could not find user");
    }
    if(response) {
        return response.data;
    }
}

const tabs = ref([
    {id: "chats", name: "Chats"},
    {id: "contacts", name: "Contacts"}
]);

const activeTab = ref(tabs.value[0].id);

const setActiveTab = (tabId: string) => {
    activeTab.value = tabId;
}

const selectContact = (contact:ContactBasicInfo) => {
    emit("contactSelected", contact);
}

const selectChat = (chat:ChatMessageBasicInfo) => {
    emit("chatSelected", chat);
}

onMounted(() => {
    $("#myTab a").on("click", function(event) {
        event.preventDefault();
        $(this).tab("show");
    });
});

</script>

<template>
    <nav id="myTab" class="nav nav-tabs flex-sm-row">
        <a v-for="(tab, index) in tabs"
            :key="index"
             class="nav-link nav-item py-2 px-4" 
            :class="{active: tab.id == activeTab}" 
            :id="`#${tab.id}-tab`"
            @click.prevent="setActiveTab(tab.id)"
            data-toggle="tab" 
            href="#" 
            role="tab"
            :aria-controls="tab.id"
            :aria-selected="tab.id == activeTab">
            {{ tab.name }}
        </a>
    </nav>
    <div class="tab-content" id = "myTabContent">
        <div v-for="(tab, index) in tabs"
            class="tab-pane fade" 
            :class="{ show: tab.id === activeTab, active: tab.id === activeTab }" 
            :key = "index" 
            :id="tab.id" 
            role="tabpanel" 
            :aria-labelledby="`#${tab.id}-tab`">
            <div v-if="activeTab == 'chats'">
                <div v-if ="props.chats && props.chats.length > 0">
                    <ul>
                        <li v-for="(chat, index) in props.chats" :key="index">
                            <a href="#" @click.prevent="selectChat(chat)">
                                chat with {{ getUserInfoById(chat.chatMessage.recipientUserId).then((user) => user?.user.username) }}
                            </a>
                        </li>
                    </ul>
                </div>
                <p v-if="props.chats?.length == 0">Chats will go here</p>
            </div>
            <div v-if="activeTab == 'contacts'">
                <ul v-if="props.contacts && props.contacts.length > 0">
                    <li v-for="(contact, index) in props.contacts" :key="index">
                        <a href="#" @click.prevent="selectContact(contact)">
                            {{ contact.contact.contactName }}
                        </a>
                    </li>
                </ul>
                <p v-if="props.contacts?.length == 0">No contacts yet</p>
            </div>
        </div>
    </div>
</template>

<style scoped>
.nav-tabs .nav-link {
    cursor: pointer;
    display: flex;
    flex-direction: row;
}
</style>