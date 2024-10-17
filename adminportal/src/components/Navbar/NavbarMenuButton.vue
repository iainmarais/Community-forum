<script lang="ts" setup>
import { ref, watch } from 'vue';
import { VBtn, VMenu, VList, VListItem, VListGroup } from 'vuetify/lib/components/index.mjs';
import { type PropType } from 'vue';
import { useAppContextStore } from '@/stores/AppContextStore';

import type { NavbarMenuItem, NavbarLinkItem, NavbarSubmenuItem, NavbarItem } from '@/stores/AppContextStore'; // Adjust the import path as needed

const navMenu = ref<NavbarMenuItem>();
const navMenuItems = ref<NavbarItem[]>([]);
const appContextStore = useAppContextStore();

const props = defineProps({
  menuItem: {
    type: Object as PropType<NavbarMenuItem>,
    required: true,
  },
});

const isOpen = ref(false);

const openMenu = () => {
  isOpen.value = !isOpen.value;
  console.log(isOpen.value);
}
//Need to check if the user is logged in and if true, remove the "Log in" option from the menu

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

</script>

<template>
    <div>
        <VBtn v-if="navMenu" @click="openMenu()" class="navbar-button" :prepend-icon="navMenu.iconClass">
            {{ navMenu.label }}
        </VBtn>
        <VMenu v-if="navMenu" v-model="isOpen" :close-on-content-click="true" transition="scale-transition" offset-y min-width="auto">
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
    </div>
</template>

<style scoped lang = "scss">
$navy-dark: darken(navy, 16%);
$navy-light: darken(navy, 6%);
$primary-color: #0c038a;
$primary-hover: #0056b3;
$primary-active: #003f7f;
$text-color: #fff;
$font-family: Arial, Helvetica, sans-serif;
$border-radius: 5px;


.navbar-button {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100px;
    height: 40px;
    border-radius: $border-radius;
    background-color: $primary-color;
    color: $text-color;
    font-family: $font-family;
    font-size: 12px;
    text-decoration: none;
    text-transform: capitalize;
    cursor: pointer;
    transition: background-color 0.3s ease;
    &:hover {
        background-color: $primary-hover;
    }
    &:active {
        background-color: $primary-active;
    }
}

.navbar-button-sm {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100px;
    height: 30px;
    border-radius: $border-radius;
    background-color: $primary-color;
    color: $text-color;
    font-family: $font-family;
    font-size: 12px;
    text-decoration: none;
    text-transform: capitalize;
    cursor: pointer;
    transition: background-color 0.3s ease;
    &:hover {
        background-color: $primary-hover;
    }
    &:active {
        background-color: $primary-active;
    }
}

.navbar-menu-item {
  font-family: $font-family;
  font-size: 12px;
  text-transform: capitalize;
  cursor: pointer;

  &:hover {
    background-color: $primary-hover;
  }

  &:active {
    background-color: $primary-active;
  }
}

</style>