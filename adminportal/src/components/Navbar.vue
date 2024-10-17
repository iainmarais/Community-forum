<script lang = "ts" setup>
import { useAppContextStore } from '@/stores/AppContextStore';
import { onMounted, onUnmounted, ref } from 'vue';
import AppLogo from './elements/AppLogo.vue';
import { VBtn, VMenu, VList, VListItem, VListGroup } from 'vuetify/lib/components/index.mjs';
import BSNavbarMenuButton from '@/components/Navbar/BSNavbarMenuButton.vue';
import BSNavbarLinkButton from './Navbar/BSNavbarLinkButton.vue';

const appContextStore = useAppContextStore();

const isWidescreen = ref(window.innerWidth > 1200);

const handleResize = () => {
    isWidescreen.value = window.innerWidth > 1200;
}

onMounted(() => {
    window.addEventListener("resize", handleResize);
});

onUnmounted(() => {
    window.removeEventListener("resize", handleResize);
});

const isMenuOpen = ref(false);

const hoverStates = ref(appContextStore.navbar.map(() => false));
const isActive = ref<number | null>(null);

const handleButtonClick = (index: number) => {
    // Toggle the active state if the same button is clicked again
    if (isActive.value === index) {
        isActive.value = null; // Deactivate the button
    } else {
        isActive.value = index; // Activate the clicked button
    }
};

const showMenu = () => {
    isMenuOpen.value = !isMenuOpen.value;
}

</script>

<template> 
    <nav class="navbar navbar-expand-lg navbar-background" style="position: relative;">
        <div class="container-fluid">
            <a href="#" class="navbar-brand">
                <AppLogo size="small" />
                <span class="navbar-brand-text" style="margin-left: 10px; color:darkseagreen">Admin Portal</span>
            </a>          
            <div class="navbar-light">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li v-for="(navbarItem, index) in appContextStore.navbar" :key="navbarItem.id || navbarItem.label">
                        <div v-if="isWidescreen && navbarItem.type === 'item'" class="navbar-element">
                            <BSNavbarLinkButton :dstName="navbarItem.routename" :iconClass="navbarItem.iconClass" :labelText="navbarItem.label" />
                        </div>
                        <div v-else-if="!isWidescreen && navbarItem.type === 'menu'" class="nav-item dropdown">
                            <BSNavbarMenuButton :menu-item="navbarItem" />
                        </div> 
                    </li>
                </ul>
            </div>
        </div>
    </nav> 
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