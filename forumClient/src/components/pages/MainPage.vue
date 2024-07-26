<script lang = "ts" setup>
import router, { HomeRoute, LoginRoute, MainRoute } from '@/router';
import { onMounted, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { useAppContextStore, type NavbarItem } from '@/stores/AppContextStore';
import { useMainPageStore } from '@/stores/MainPageStore';
import { TokenKey } from '@/LocalStorage/keys';
import { SetToken } from '@/http/AxiosClient';
import AppLogo from '@/components/elements/AppLogo.vue';

const appContextStore = useAppContextStore();
const mainPageStore = useMainPageStore();

import LoadingIndicator from '../elements/LoadingIndicator.vue';
import ConfigurationLoader from '@/config/ConfigurationLoader';
import UserProfileModal from '@/components/modals/UserProfileModal.vue';
import LoginPage from './LoginPage.vue';

const route = useRoute();

const showNavigationMenu = ref<boolean>(false);

onMounted(() => {
    const storedToken = localStorage.getItem(TokenKey);
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
        <div id="kt_header_mobile" class="header-mobile header-mobile-fixed">
            <div class="d-flex align-items-center">
                <div class="dropdown">
                    <button class="btn btn-icon ml-4" id="kt_header_mobile_toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <i class="fa fa-bars"></i>
                    </button>
                    <div class="dropdown-menu">
                        <a :id="navbarItem.id" v-for="navbarItem in appContextStore.navbar" :class="getNavItemClass(navbarItem)" class="dropdown-item">
                            <RouterLink v-if="navbarItem.type === 'item'"  :to="{ name: navbarItem.routename, params: navbarItem.routeParams }" class="menu-link">
                                <span class="menu-text" :class="`${navbarItem.labelClass || ''}`">
                                    <i :class="`${navbarItem.iconClass || ''} nav-menu-icon`"></i>
                                    {{ navbarItem.label }}
                                </span>
                            </RouterLink>
                            <a v-if="navbarItem.type == 'menu'" htef="javascript:;" class="menu-link menu-toggle">
                                <span class="menu-text" :class="`${navbarItem.labelClass || ''}`">
                                    <i :class="`${navbarItem.iconClass || ''} nav-menu-icon`"></i>
                                    {{ navbarItem.label }}
                                </span>
                            </a> 
                            <div v-if="navbarItem.type === 'menu'" class="menu-submenu menu-submenu-classic menu-submenu-left" @mouseleave="closeSubmenu(navbarItem.id)">
                                <ul class="menu-subnav">
                                    <li class="menu-item" :id="subMenuItem.id"
                                        @mouseleave="onNavitemToggle(subMenuItem)"
                                        @mouseenter="onNavitemToggle(subMenuItem)"
                                        :class="subMenuItem.type === 'submenu' ? 'menu-item-submenu menu-subsub-menu' : ''"
                                        aria-haspopup="true" v-for="subMenuItem in navbarItem.items">
                                        <RouterLink v-if="subMenuItem.type == 'item'"
                                            :to="{ name: subMenuItem.routename, params: subMenuItem.routeParams }"
                                            class="menu-link">
                                            <span class="menu-text"
                                                :class="`${navbarItem.labelClass || ''}`"><i
                                                    :class="subMenuItem.iconClass"></i>{{ subMenuItem.label
                                                    }}</span><span
                                                class="label label-light-primary label-rounded font-weight-bold label-inline"
                                                v-if="appContextStore.getNavbarItemState(subMenuItem)">{{
                                                    appContextStore.getNavbarItemState(subMenuItem) }}</span>
                                        </RouterLink>
                                        <span v-if="subMenuItem.type == 'submenu'" class="menu-link">
                                            <span class="nav-menu-icon">
                                                <i :class="subMenuItem.iconClass"></i>
                                            </span>
                                            <span class="menu-text">{{ subMenuItem.label }}</span>
                                            <i class="menu-arrow"></i>
                                        </span>
                                        <div v-if="subMenuItem.type == 'submenu'"
                                            class="menu-submenu menu-submenu-classic menu-submenu-right"
                                            data-hor-direction="menu-submenu-right">
                                            <ul class="menu-subnav">
                                                <li class="menu-item" aria-haspopup="true"
                                                    v-for="subSubMenuItem in subMenuItem.items">
                                                    <RouterLink v-if="subSubMenuItem.type == 'item'"
                                                        :to="{ name: subSubMenuItem.routename, params: subSubMenuItem.routeParams }"
                                                        class="menu-link">
                                                        <span class="menu-text"
                                                            :class="`${subSubMenuItem.labelClass || ''}`"><i
                                                                :class="subSubMenuItem.iconClass"></i>{{
                                                                    subSubMenuItem.label }}</span><span
                                                            class="label label-light-primary label-rounded font-weight-bold label-inline"
                                                            v-if="appContextStore.getNavbarItemState(subSubMenuItem)">{{
                                                                appContextStore.getNavbarItemState(subSubMenuItem)
                                                            }}</span>
                                                    </RouterLink>
                                                </li>
                                            </ul>
                                        </div>
                                    </li>
                                </ul>
                            </div>                                                               
                        </a>                      
                    </div>
                </div>
            </div>                    
        </div>
        <div class="d-flex flex-column flex-root">
            <div class="d-flex flex-row flex-column-fluid page">
				<div class="d-flex flex-column flex-row-fluid wrapper" id="kt_wrapper">
                    <div id="kt_header" class="header header-fixed">
                        <div class="container-fluid">
                            <div class="header-menu-wrapper header-menu-wrapper-left" id="kt_header_menu_wrapper">
                                <div class="">
                                    <AppLogo size="small" class="header-logo" :light="false" />
                                </div>
                                <div id="kt_header_menu" class="header-menu header-menu-mobile header-menu-layout-default">
                                    <ul class="menu-nav">
                                        <li :id="navbarItem.id" v-for="navbarItem in appContextStore.navbar" :class="getNavItemClass(navbarItem)">
                                            <RouterLink v-if="navbarItem.type === 'item'" 
                                            :to="{ name: navbarItem.routename, params: navbarItem.routeParams }" 
                                            class="menu-link">
                                                <span class="menu-text" :class="`${navbarItem.labelClass || ''}`">
                                                    <i :class="`${navbarItem.iconClass || ''} nav-menu-icon`"></i>
                                                     {{ navbarItem.label }}
                                                </span>
                                            </RouterLink>
                                            <a v-if="navbarItem.type == 'menu'" htef="javascript:;" class="menu-link menu-toggle">
                                                <span class="menu-text" :class="`${navbarItem.labelClass || ''}`">
                                                    <i :class="`${navbarItem.iconClass || ''} nav-menu-icon`"></i>
                                                    {{ navbarItem.label }}
                                                </span>
                                            </a>
                                            <div v-if="navbarItem.type === 'menu'"
												class="menu-submenu menu-submenu-classic menu-submenu-left"
												@mouseleave="closeSubmenu(navbarItem.id)">
												<ul class="menu-subnav">
													<li class="menu-item" :id="subMenuItem.id"
														@mouseleave="onNavitemToggle(subMenuItem)"
														@mouseenter="onNavitemToggle(subMenuItem)"
														:class="subMenuItem.type === 'submenu' ? 'menu-item-submenu menu-subsub-menu' : ''"
														aria-haspopup="true" v-for="subMenuItem in navbarItem.items">
														<RouterLink v-if="subMenuItem.type == 'item'"
															:to="{ name: subMenuItem.routename, params: subMenuItem.routeParams }"
															class="menu-link">
															<span class="menu-text"
																:class="`${navbarItem.labelClass || ''}`"><i
																	:class="subMenuItem.iconClass"></i>{{ subMenuItem.label
																	}}</span><span
																class="label label-light-primary label-rounded font-weight-bold label-inline"
																v-if="appContextStore.getNavbarItemState(subMenuItem)">{{
																	appContextStore.getNavbarItemState(subMenuItem) }}</span>
														</RouterLink>
														<span v-if="subMenuItem.type == 'submenu'" class="menu-link">
															<span class="nav-menu-icon">
																<i :class="subMenuItem.iconClass"></i>
															</span>
															<span class="menu-text">{{ subMenuItem.label }}</span>
															<i class="menu-arrow"></i>
														</span>
														<div v-if="subMenuItem.type == 'submenu'"
															class="menu-submenu menu-submenu-classic menu-submenu-right"
															data-hor-direction="menu-submenu-right">
															<ul class="menu-subnav">
																<li class="menu-item" aria-haspopup="true"
																	v-for="subSubMenuItem in subMenuItem.items">
																	<RouterLink v-if="subSubMenuItem.type == 'item'"
																		:to="{ name: subSubMenuItem.routename, params: subSubMenuItem.routeParams }"
																		class="menu-link">
																		<span class="menu-text"
																			:class="`${subSubMenuItem.labelClass || ''}`"><i
																				:class="subSubMenuItem.iconClass"></i>{{
																					subSubMenuItem.label }}</span><span
																			class="label label-light-primary label-rounded font-weight-bold label-inline"
																			v-if="appContextStore.getNavbarItemState(subSubMenuItem)">{{
																				appContextStore.getNavbarItemState(subSubMenuItem)
																			}}</span>
																	</RouterLink>
																</li>
															</ul>
														</div>
													</li>
												</ul>
											</div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="topbar">
                                <div class="topbar-item" data-offset="10px, 0px" aria-expanded="false">
                                    <div class="btn btn-hover-transparent-white btn-lg btn-icon mr-1"
                                        @click="">
                                        <i class="fa fa-search text-primary"></i>
                                    </div>
                                </div>
                                <div class="topbar-item mr=2">
                                    <div class="btn btn-icon btn-hover-transparent-white w-auto d-flex align-items-center btn-lg px-2"
                                        @click="showUserProfileModal()">
                                        <div class="symbol symbol-30 bg-white overflow-hidden">
                                            <div class="symbol-label">
                                                <i class="fas fa-user fa-lg"></i>
                                            </div>  
                                        </div>
                                    </div>
                                    <span class="label label-warning label-lg label-inline font-weight-bolder" v-if="ConfigurationLoader.getConfig().envName=='local'">
                                        {{ ConfigurationLoader.getConfig().envName }}                                        
                                    </span>
                                    <span class="label label-danger label-lg label-inline font-weight-bolder" v-if="appContextStore.loggedInUser?.roleName != null">
                                        {{ appContextStore.loggedInUser?.roleName }}
                                    </span>
                                    <span class="label label-danger label-lg label-inline font-weight-bolder" v-if="!appContextStore.loggedInUser">
                                        Guest
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="subheader py-2" id="kt_subheader" v-if="mainPageStore.breadcrumbs.length > 0">
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
					<div class="content d-flex flex-column flex-column-fluid" id="kt_content">
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