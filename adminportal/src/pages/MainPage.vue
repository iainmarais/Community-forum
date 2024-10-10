<script lang = "ts" setup>
import router, { HomeRoute } from '@/router';
import { onMounted, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import ConfigurationLoader from '@/config/ConfigurationLoader';
import NavigationBar from '@/components/NavigationBar.vue';
import { useMainPageStore } from '@/stores/MainPageStore';
import { Token_Key } from '@/LocalStorage/keys';
import { SetToken } from '@/http/AxiosClient';
import { useAppContextStore } from '@/stores/AppContextStore';

const route = useRoute();

const appContextStore = useAppContextStore();
const mainPageStore = useMainPageStore();

onMounted(() => {
    const storedToken = localStorage.getItem(Token_Key);
    if(storedToken) {
        SetToken(storedToken);
    }
    appContextStore.getAppState();
});


</script>

<template>
    <div class="d-flex flex-column flex-root">
        <div class="d-flex flex-row flex-column-fluid page">
            <div class="d-flex flex-column flex-row-fluid">
                <NavigationBar />
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
            <div class="content d-flex flex-column flex-column-fluid">
                <div class="d-flex flex-column-fluid">
                    <div class="container">
                        <RouterView :key="route.fullPath" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>