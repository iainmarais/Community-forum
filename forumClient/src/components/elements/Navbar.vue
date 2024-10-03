<script lang = "ts" setup>
import { useRouter, useRoute } from 'vue-router';
import { useAppContextStore, type NavbarItem, type NavbarLinkItem, type NavbarSubmenuItem, type NavbarMenuItem } from '@/stores/AppContextStore';
import AppLogo from '@/components/elements/AppLogo.vue';
import { onMounted, onUnmounted, ref, watch } from 'vue';
import { LoginRoute, MainRoute } from '@/router';
import { useNavigationStore } from '@/stores/NavigationStore';
import Searchbar from '@/components/elements/Navbar/Searchbar.vue';
import UserProfileNavbarElement from '@/components/elements/Navbar/UserProfileNavbarElement.vue';
import NavbarLinkButton from './Navbar/NavbarLinkButton.vue';
import NavbarMenuButton from './Navbar/NavbarMenuButton.vue';

const appContextStore = useAppContextStore();
const navigationStore = useNavigationStore();
const router = useRouter();
const route = useRoute();

watch(() => route, _ => {
	navigationStore.setBreadcrumbs([]);
}, { flush: 'pre', immediate: true, deep: true });


watch(() => appContextStore.loggedInUser, (newValue) => {
    if(newValue) {
        router.push(MainRoute);
    }
    if(!newValue) {
        router.push(LoginRoute);
    }
});

const isWidescreen=ref(window.innerWidth> 1200);

const handleResize = () => {
    isWidescreen.value = window.innerWidth> 1200;
}
onMounted(() => {
    window.addEventListener('resize', handleResize);
});

onUnmounted(() => {
    window.removeEventListener('resize', handleResize);
});
</script>

<template>
    <div class="navbar">
      <div class="navbar-inner">
        <div class="navbar-brand">
          <AppLogo :size="'small'" />
        </div>
        <div class="navbar-content">
            <ul class="navbar-items">
                <div v-for="navbarItem in appContextStore.navbar">
                    <NavbarMenuButton v-if="!isWidescreen && navbarItem.type==='menu'" :menuItem="navbarItem" />
                    <NavbarLinkButton v-if="isWidescreen && navbarItem.type==='item'" :key="navbarItem.id" :dstName="navbarItem.routename" :dstParams="navbarItem.routeParams" :iconClass="navbarItem.iconClass" :labelText="navbarItem.label" />
                </div>               
            </ul>
            <!--search function-->
            <div class="navbar-search">
                <Searchbar />
            </div>
            <div class=".navbar-userprofile">
                <!--This element needs fixing.-->
                <UserProfileNavbarElement />
            </div>
        </div>
      </div>
    </div>
  </template>