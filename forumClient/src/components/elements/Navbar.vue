<script lang = "ts" setup>
import { useRouter, useRoute } from 'vue-router';
import { useAppContextStore, type NavbarItem, type NavbarLinkItem, type NavbarSubmenuItem, type NavbarMenuItem } from '@/stores/AppContextStore';
import AppLogo from '@/components/elements/AppLogo.vue';
import { watch } from 'vue';
import { LoginRoute, MainRoute } from '@/router';
import { useNavigationStore } from '@/stores/NavigationStore';
import Searchbar from '@/components/elements/Navbar/Searchbar.vue';
import UserProfileNavbarElement from '@/components/elements/Navbar/UserProfileNavbarElement.vue';

const appContextStore = useAppContextStore();
const navigationStore = useNavigationStore();
const router = useRouter();
const route = useRoute();

// watch(() => router, _ => {
//     navigationStore.setBreadcrumbs([]);
// }, {flush: 'pre', immediate: true, deep: true });

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


// const handleNavbarClick = (item: NavbarItem) => {
//     if(item == item as NavbarLinkItem) {
//             switch(item.id)
//         {   
//             case "logoff":
//                 appContextStore.logoff();
//             default:
//                 router.push(item.routename);
//         }
//     }
// }

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
    // if(navitem.type=="item") {
    //     handleNavbarClick(navitem);
    // }
}

const closeSubmenu = (id: string) => {
    const elem = $(`#${id}`);
    const openClass = "menu-item-hover";
    const hasClass = elem.hasClass(openClass);
    if(hasClass) {
        elem.removeClass(openClass);
    }
}

</script>

<template>
    <div class="navbar">
      <div class="navbar-inner">
        <div class="navbar-brand">
          <AppLogo :size="'small'" />
        </div>
        <div class="navbar-content">
            <ul class="navbar-items">
                <li :id="navbarItem.id" v-for="navbarItem in appContextStore.navbar" class="navbar-items li" @click="onNavitemToggle(navbarItem)">
                <RouterLink v-if="navbarItem.type === 'item'" :to="{ name: navbarItem.routename, params: navbarItem.routeParams }">
                    <span class="navbar-button-text" :class="`${navbarItem.labelClass || ''}`">
                    <i :class="`${navbarItem.iconClass || ''} nav-menu-icon`"></i>
                    {{ navbarItem.label }}
                    </span>
                </RouterLink>
                <a v-if="navbarItem.type == 'menu'" href="javascript:;" class="navbar-items li a">
                    <span class="navbar-button-text" :class="`${navbarItem.labelClass || ''}`">
                    <i :class="`${navbarItem.iconClass || ''} nav-menu-icon`"></i>
                    {{ navbarItem.label }}
                    </span>
                </a>
                <div v-if="navbarItem.type === 'menu'" class="menu-submenu menu-submenu-classic menu-submenu-left" @mouseleave="closeSubmenu(navbarItem.id)">
                    <ul class="menu-subnav">
                    <li class="menu-item" :id="subMenuItem.id" @mouseleave="onNavitemToggle(subMenuItem)" @mouseenter="onNavitemToggle(subMenuItem)" :class="subMenuItem.type === 'submenu' ? 'menu-item-submenu menu-subsub-menu' : ''" aria-haspopup="true" v-for="subMenuItem in navbarItem.items">
                        <RouterLink v-if="subMenuItem.type == 'item'" :to="{ name: subMenuItem.routename, params: subMenuItem.routeParams }" class="menu-link">
                        <span class="menu-text" :class="`${navbarItem.labelClass || ''}`"><i :class="subMenuItem.iconClass"></i>{{ subMenuItem.label }}</span>
                        <span class="label label-light-primary label-rounded font-weight-bold label-inline" v-if="appContextStore.getNavbarItemState(subMenuItem)">{{ appContextStore.getNavbarItemState(subMenuItem) }}</span>
                        </RouterLink>
                        <span v-if="subMenuItem.type == 'submenu'" class="menu-link">
                        <span class="nav-menu-icon">
                            <i :class="subMenuItem.iconClass"></i>
                        </span>
                        <span class="menu-text">{{ subMenuItem.label }}</span>
                        <i class="menu-arrow"></i>
                        </span>
                        <div v-if="subMenuItem.type == 'submenu'" class="menu-submenu menu-submenu-classic menu-submenu-right" data-hor-direction="menu-submenu-right">
                        <ul class="menu-subnav">
                            <li class="menu-item" aria-haspopup="true" v-for="subSubMenuItem in subMenuItem.items">
                            <RouterLink v-if="subSubMenuItem.type == 'item'" :to="{ name: subSubMenuItem.routename, params: subSubMenuItem.routeParams }" class="menu-link">
                                <span class="menu-text" :class="`${subSubMenuItem.labelClass || ''}`"><i :class="subSubMenuItem.iconClass"></i>{{ subSubMenuItem.label }}</span>
                                <span class="label label-light-primary label-rounded font-weight-bold label-inline" v-if="appContextStore.getNavbarItemState(subSubMenuItem)">{{ appContextStore.getNavbarItemState(subSubMenuItem) }}</span>
                            </RouterLink>
                            </li>
                        </ul>
                        </div>
                    </li>
                    </ul>
                </div>
                </li>
            </ul>
            <!--search function-->
            <div class="navbar-search">
                <Searchbar />
            </div>
            <div class=".navbar-userprofile">
                <UserProfileNavbarElement  />
            </div>
        </div>
      </div>
    </div>
  </template>