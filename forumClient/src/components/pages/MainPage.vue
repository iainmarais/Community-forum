<script lang = "ts" setup>
import router, { HomeRoute, LoginRoute, MainRoute } from '@/router';
import { onMounted, watch } from 'vue';
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
import LoginPage from './LoginPage.vue';

const route = useRoute();

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
})

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

const toggleUserSidePanel = () => {
    $('#userSidePanel').toggleClass('offcanvas-on');
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

</script>

<template>
    <div v-if="!appContextStore.appLoading">
        <div id="kt_header_mobile" class="header-mobile header-mobile-fixed">
            <div class="d-flex align-items-center">
                <button class="btn btn-icon ml-4" id="kt_header_mobile_toggle">
					<span class="svg-icon svg-icon-xxl">
						<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px"
							height="24px" viewBox="0 0 24 24" version="1.1">
							<g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
								<rect x="0" y="0" width="24" height="24" />
								<rect fill="#000000" x="4" y="4" width="7" height="7" rx="1.5" />
								<path
									d="M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z"
									fill="#000000" opacity="0.3" />
							</g>
						</svg>
					</span>
				</button>
                <button class="btn btn-icon ml-2" id="kt_header_mobile_topbar_toggle">
					<span class="svg-icon svg-icon-xl">
						<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px"
							height="24px" viewBox="0 0 24 24" version="1.1">
							<g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
								<polygon points="0 0 24 0 24 24 0 24" />
								<path
									d="M12,11 C9.790861,11 8,9.209139 8,7 C8,4.790861 9.790861,3 12,3 C14.209139,3 16,4.790861 16,7 C16,9.209139 14.209139,11 12,11 Z"
									fill="#000000" fill-rule="nonzero" opacity="0.3" />
								<path
									d="M3.00065168,20.1992055 C3.38825852,15.4265159 7.26191235,13 11.9833413,13 C16.7712164,13 20.7048837,15.2931929 20.9979143,20.2 C21.0095879,20.3954741 20.9979143,21 20.2466999,21 C16.541124,21 11.0347247,21 3.72750223,21 C3.47671215,21 2.97953825,20.45918 3.00065168,20.1992055 Z"
									fill="#000000" fill-rule="nonzero" />
							</g>
						</svg>
					</span>
				</button>
            </div>                    
        </div>
        <div class="d-flex flex-column flex-root">
            <div class="d-flex flex-row flex-column-fluid page">
				<div class="d-flex flex-column flex-row-fluid wrapper" id="kt_wrapper">
                    <div id="kt_header" class="header header-fixed">
                        <div class="container-fluid">
                            <div class="header-menu-wrapper header-menu-wrapper-left" id="kt_header_menu_wrapper">
                                <!--Logo placeholder - leave empty for now-->
                                <div class="">
                                    <AppLogo size="small" class="header-logo" :light="false" />
                                </div>
                                <!--Nav bar-->
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
                                <!--Global search modal-->
                                <div class="topbar-item" data-offset="10px, 0px" aria-expanded="false">
                                    <div class="btn btn-hover-transparent-white btn-lg btn-icon mr-1"
                                        @click="">
                                        <i class="fa fa-search text-primary"></i>
                                    </div>
                                </div>
                                <!--User profile dropdown-->
                                <div class="topbar-item mr=2">
                                    <div class="btn btn-icon btn-hover-transparent-white w-auto d-flex align-items-center btn-lg px-2"
                                        @click="toggleUserSidePanel()">
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
        <div id="userSidePanel" class="offcanvas offcanvas-right p-10">
            <div class="offcanvas-header d-flex align-items-center justify-content-between pb-5" style="" kt-hidden-height="40">
                <h3 class="font-weight-bold m-0">User profile</h3>
                <button type="button" class="btn btn-xs btn-icon btn-light btn-hover-primary" @click="toggleUserSidePanel()">
                    <i class="ki ki-close icon-xs text-muted"></i>
                </button>
            </div>
        </div>
    </div>
    <div v-else class="text-center mt-20">
        <AppLogo size="large" />
        <LoadingIndicator class="mt-10" :loading="true" />
    </div>
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