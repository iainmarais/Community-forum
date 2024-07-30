<script lang = "ts" setup>
import router, { HomeRoute, LoginRoute, MainRoute } from '@/router';
import { onMounted, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { useAppContextStore, type NavbarItem } from '@/stores/AppContextStore';
import { useMainPageStore } from '@/stores/MainPageStore';
import { Token_Key } from '@/LocalStorage/keys';
import { SetToken } from '@/http/AxiosClient';
import AppLogo from '@/components/elements/AppLogo.vue';
import "@/assets/scss/pagestyles.scss";
import Navbar from '@/components/elements/Navbar.vue';

const appContextStore = useAppContextStore();
const mainPageStore = useMainPageStore();

import LoadingIndicator from '../elements/LoadingIndicator.vue';
import ConfigurationLoader from '@/config/ConfigurationLoader';
import UserProfileModal from '@/components/modals/UserProfileModal.vue';

const route = useRoute();

const showNavigationMenu = ref<boolean>(false);

onMounted(() => {
    const storedToken = localStorage.getItem(Token_Key);
    if(storedToken) {
        SetToken(storedToken);
    }
    appContextStore.getAppState();
});

watch(() => router, _ => {
    mainPageStore.setBreadcrumbs([]);
}, {flush: 'pre', immediate: true, deep: true });

watch(() => appContextStore.loggedInUser, (newValue) => {
    if(newValue) {
        router.push(MainRoute);
    }
    if(!newValue) {
        router.push(LoginRoute);
    }
});



const handleNavbarClick = (item: NavbarItem) => {
    switch(item.id)
    {   
        case "logoff":
            appContextStore.logoff();
        default:
            return;
    }
}

const toggleSubmenu = (id: string) => {
    const elem = $(`#${id}`);
    const openClass = "menu-item-hover";
    const hasclass = elem.hasClass(openClass);
    const allDropdowns = $('menu-item-submenu.top-nav-level-menu');

    if(hasclass) {
        elem.removeClass(openClass);
    }
    else {
        allDropdowns.removeClass(openClass);
        elem.addClass(openClass);
    }
}

const showUserProfileModal = () => {
    $('#userProfileModal').modal("show");
}

const closeSubmenu = (id: string) => {
    const elem = $(`#${id}`);
    const openClass = "menu-item-hover";
    const hasClass = elem.hasClass(openClass);
    if(hasClass) {
        elem.removeClass(openClass);
    }
}

const getNavItemClass = (navitem: NavbarItem) => {
    if(navitem.type === "item") {
        return `menu-item ${route.name == navitem.routename ? 'menu-item-active' : ''}`;
    }
    if(navitem.type==="menu") {
        return 'menu-item menu-item-submenu menu-item-rel menu-item menu-item-submenu menu-item-rel top-nav-level-menu';
    }
}

const onNavitemToggle = (navitem: NavbarItem) => {
    if (navitem.type == "menu") {
        toggleSubmenu(navitem.id);
    }
    if(navitem.type=="submenu") {
        const elem = $(`#${navitem.id}`);
        const openClass = "menu-item-hover";
        const hasClass = elem.hasClass(openClass);
        if(hasClass) {
            elem.removeClass(openClass);
        }
        else {
            elem.addClass(openClass);
        }
    }
    if(navitem.type=="item") {
        handleNavbarClick(navitem);
    }
}

const openNavigationMenu = () => {
    //Toggle the state of the navigation dropdown
    showNavigationMenu.value = !showNavigationMenu.value
}

</script>

<template>
    <div v-if="!appContextStore.appLoading">
        <div class="d-flex flex-column flex-root">
            <div class="d-flex flex-row flex-column-fluid page">
				<div class="d-flex flex-column flex-row-fluid">
                    <Navbar />
                    <div class="subheader py-2" v-if="mainPageStore.breadcrumbs.length > 0">
                        <div class="container-fluid d-flex align-items-center justify-content-between flex-wrap flex-sm-nowrap">
                            <div class="d-flex align-items-center flex-wrap mr-1">
                                <div class="d-flex align-items-baseline flex-wrap mr-5">
                                    <ul class="breadcrumb breadcrumb-transparent breadcrumb-dot font-weight-bold p-0 my-2 font-size-sm">
                                        <li class="breadcrumb-item text-muted" v-for="breadcrumb in mainPageStore.breadcrumbs">
                                            <RouterLink :to="breadcrumb.route" class="text-muted">
                                                {{ breadcrumb.text }}
                                            </RouterLink>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
					<div class="content d-flex flex-column flex-column-fluid">
						<div class="d-flex flex-column-fluid">
							<div class="container">
								<RouterView :key="route.fullPath" />
							</div>
						</div>
					</div>
                </div>
            </div>
        </div>
    </div>
    <div v-else class="text-center mt-20">
        <AppLogo size="large" />
        <LoadingIndicator class="mt-10" :loading="true" />
    </div>
<UserProfileModal />
</template>

<style>
.header-logo {
    position: relative;
    top: 50px;
    transform: translateY(-50%);
    padding-right: 0px;
}

.header-menu-wrapper .header-logo {
	background-color: #1D1F32;
	padding-right: 0px;
}

</style>