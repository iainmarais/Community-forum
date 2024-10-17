<script lang ="ts" setup>
import { useAppContextStore, type NavbarItem, type NavbarMenuItem } from '@/stores/AppContextStore';
import { ref, watch, type PropType } from 'vue';
import { VMenu, VList, VListItem, VListGroup } from 'vuetify/lib/components/index.mjs';


const props = defineProps({
    menuItem: {
        type: Object as PropType<NavbarMenuItem>,
        required: true,
    },
});

const navMenu = ref<NavbarMenuItem>();
const navMenuItems = ref<NavbarItem[]>([]);
const appContextStore = useAppContextStore();
const isMenuOpen = ref<boolean>(false);

watch(() => navMenu.value, (newValue) => {
    const items: NavbarItem[] = [];
    for(const item of newValue!.items) {
        if(appContextStore.currentLoggedInUser && item.id=="login") {
                continue; 
            }
        if(appContextStore.currentLoggedInUser==undefined && item.id=="logoff") {
                continue;
            }
            items.push({...item});
        }
    navMenuItems.value = items;
    }, 
{ flush: 'pre', immediate: true, deep: true });

watch (() => props.menuItem, (newValue) => {
  navMenu.value = newValue;
}, { flush: 'pre', immediate: true, deep: true });


const showMenu = () => {
    isMenuOpen.value = !isMenuOpen.value;
}

</script>

<template>
    <button v-if="navMenu" 
    type="button" 
    class="btn btn-primary btn-sm dropdown-toggle" 
    @click="showMenu" 
    id="navbarDropdownMenuButton" 
    role="button" 
    data-bs-toggle="dropdown" 
    aria-expanded="false">
        <i :class="navMenu.iconClass"></i>
        {{ navMenu.label }}
    </button>
    <!--Needs a bit of work still. The menu works, but doesn't render right. It shows at the top-left corner of the screen not under the button.-->
    <VMenu v-model="isMenuOpen" :close-on-content-click="true" transition="scale-transition" offset-y min-width="auto">
        <VList>
            <VListGroup v-for="subitem in navMenuItems" :key="subitem.label">
                <template v-slot:activator="{ props }">
                    <VListItem v-bind="props" v-if="subitem.type==='item'" :to="{name: subitem.routename, params: subitem.routeParams}">
                        {{ subitem.label }}
                    </VListItem>      
                </template>
            </VListGroup>
        </VList>
    </VMenu>
</template>

<style lang="scss">
.dropdown-menu {
    //Dropdown menu should appear as an overlay, but how to get this to render properly?
    position: absolute;
}

.dropdown-menu.show {
    //Show only when active
    display: block;
}

.navbar-link {
    text-decoration: none;
    color: rgb(255, 255, 255);
    font-size: 14px;
    font-weight: 500;
    display: flex;
}

.navbar-link.hover {
    color: rgb(255, 255, 255);
}

.navbar-brand-text {
    color: rgb(255, 255, 255);
    font-size: 16px;
    font-weight: bold;
}

.navbar-element {
    //spacing between them of around 10px
    padding: 0 10px;
}

.navbar-button {
    display: flex;
    background-color: navy;
}

.navbar-button.hover {
    background-color: darkslateblue;
}

.navbar-button.active {
    background-color: rgb(57, 34, 207);
}

.navbar-background {
    background-color: rgb(35, 20, 53);
}

</style>