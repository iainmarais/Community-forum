<script lang="ts" setup>
import router, { HomeRoute, MainRoute, LoginRoute } from '@/router';
import { onMounted, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import ConfigurationLoader from '@/config/ConfigurationLoader';
import Navbar from '@/components/Navbar.vue';
import { useMainPageStore } from '@/stores/MainPageStore';
import { Token_Key } from '@/LocalStorage/keys';
import { SetToken } from '@/http/AxiosClient';
import { useAppContextStore } from '@/stores/AppContextStore';
import AppLogo from '@/components/elements/AppLogo.vue';
import LoadingIndicator from '@/components/LoadingIndicator.vue';

const route = useRoute();

const appContextStore = useAppContextStore();
const mainPageStore = useMainPageStore();

onMounted(async () => {
    try {
        const storedToken = localStorage.getItem(Token_Key);
        if (storedToken) {
            SetToken(storedToken);
            await appContextStore.getAppState();
        } else {
            appContextStore.appLoading = false;
            router.push(LoginRoute);
        }
    } catch (error) {
        console.error('Error initializing app:', error);
        appContextStore.appLoading = false;
        router.push(LoginRoute);
    }
});

watch(() => appContextStore.currentLoggedInUser, (newValue, oldValue) => {
    if (newValue) {
        // User logged in - ensure we're on main route
        if (route.name === LoginRoute) {
            router.push(MainRoute);
        }
    } else if (oldValue) { // Only redirect if we're transitioning from logged in to logged out
        // User logged out - ensure we're on login route
        router.push(LoginRoute);
    }
}, { immediate: true });

watch(() => route.fullPath, () => {
    if (!appContextStore.appLoading && appContextStore.currentLoggedInUser) {
        mainPageStore.setBreadcrumbs([]);
    }
}, { immediate: true });

</script>

<template>
    <div v-if="!appContextStore.appLoading">
        <div class="d-flex flex-column flex-root">
            <!--Navigation area-->
            <div class="d-flex flex-row flex-column-fluid page">
                <div class="d-flex flex-column flex-row-fluid">
                    <Navbar />
                </div>
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
            </div>
            <!--Content area-->
            <div class="content d-flex flex-column flex-column-fluid">
                <div class="d-flex flex-column-fluid">
                    <div class="container">
                        <RouterView :key="route.fullPath" />
                    </div>
                </div>
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