<script lang = "ts" setup>
import { useAppContextStore } from '@/stores/AppContextStore';
import { onMounted, onUnmounted, ref } from 'vue';
import NavbarLinkButton from './Navbar/NavbarLinkButton.vue';
import NavbarMenuButton from './Navbar/NavbarMenuButton.vue';

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

</script>

<template> 
    <div class = "navbar">
        <div class = "navbar-inner">
            <div class="navbar-brand">
                <a href = "#">Admin Portal</a>
            </div>
            <div class="navbar-content">
                <ul class="navbar-items">
                    <div v-for="navbarItem in appContextStore.navbar">
                        <NavbarMenuButton v-if="!isWidescreen && navbarItem.type==='menu'" :menuItem="navbarItem" />
                        <NavbarLinkButton v-if="isWidescreen && navbarItem.type==='item'" :key="navbarItem.id" :dstName="navbarItem.routename" :dstParams="navbarItem.routeParams" :iconClass="navbarItem.iconClass" :labelText="navbarItem.label" />
                    </div>
                </ul>
            </div>
        </div>
    </div>

</template>

<style>
@import "@/assets/scss/navbar.scss";
</style>