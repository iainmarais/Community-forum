<script lang = "ts" setup>
import { computed } from 'vue';
import { useAppContextStore } from '@/stores/AppContextStore';

const appContextStore = useAppContextStore();
const navbar = computed(() => appContextStore.navbar);

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
    <nav id="navMenu">
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
    </nav>
</template>