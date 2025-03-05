<script lang="ts" setup>
import { Toast_UserLoginSuccessful } from '@/components/prepopulatedToasts';
import type { AdminLoginRequest } from '@/Dto/AdminPortal/AdminLoginRequest';
import ErrorHandler from '@/Handlers/ErrorHandler';
import { SetToken } from '@/http/AxiosClient';
import { Last_Logged_In_User_Identifier, Last_Route, Token_Key, User_Refresh_Token } from '@/LocalStorage/keys';
import router, { HomeRoute, LogoffRoute } from '@/router';
import AdminPortalService from '@/Services/AdminPortalService';
import { useAppContextStore } from '@/stores/AppContextStore';
import { useMainPageStore } from '@/stores/MainPageStore';
import { onMounted, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { useToast } from 'vue-toastification';
import Navbar from '@/components/Navbar.vue';
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';

const toast = useToast();
type LoginMethod = "identifier_password";

const userIdentifier = ref<string>("");
const password = ref<string>("");
const loading = ref<boolean>(false);

const mainPageStore = useMainPageStore();
const appContextStore = useAppContextStore();

const loginMethod = ref<LoginMethod>("identifier_password");
const loginInProgress = ref<boolean>(false);

const route = useRoute();

watch(userIdentifier, () => {
    if (localStorage.getItem(Last_Logged_In_User_Identifier)) {
        localStorage.removeItem(Last_Logged_In_User_Identifier);
    }
});

onMounted(() => {
    $(".modal-backdrop").remove();
    const LastLoggedInIdentifier = localStorage.getItem(Last_Logged_In_User_Identifier);
    if (LastLoggedInIdentifier) {
        userIdentifier.value = LastLoggedInIdentifier;
    }
    document.title = "Log in";
    if (route.params.logoffReason) {
        console.log(route.params.logoffReason);
    }
    appContextStore.buildNavbar(true);
    const storedToken = localStorage.getItem(Token_Key);
    if (storedToken) {
        loading.value = true;
        SetToken(storedToken);
        postLoginRoute();
    }
    setTimeout(() => {
        if (loginMethod.value = "identifier_password") {
            if (userIdentifier.value) {
                document.getElementById("userIdentifierInput")?.focus();
            }
            else {
                document.getElementById("passwordInput")?.focus();
            }
        }
    }, 50);
});

const LoginContext = "admin";
const ManualLogoffMethod = "manual";

const login = async () => {
    if (!userIdentifier.value || !password.value) {
        toast.error("Please enter both username and password");
        return;
    }

    const request: AdminLoginRequest = {
        userIdentifier: userIdentifier.value,
        password: password.value,
        userContext: LoginContext
    }
    
    loginInProgress.value = true;
    try {
        const response = await AdminPortalService.adminLogin(request);
        
        // Handle tokens first
        SetToken(response.data.accessToken);
        localStorage.setItem(Token_Key, response.data.accessToken);
        localStorage.setItem(Last_Logged_In_User_Identifier, request.userIdentifier);
        localStorage.setItem(User_Refresh_Token, response.data.refreshToken);
        
        // Update app state
        await appContextStore.getAppState();
        
        // Then update UI state
        loginInProgress.value = false;
        
        // Navigate after state is updated
        await postLoginRoute();

        //Reload the page to ensure the user is logged in.
        window.location.reload();        

        Toast_UserLoginSuccessful(response.data.adminUserProfile.user.username);
    } catch (error) {
        loginInProgress.value = false;
        ErrorHandler.handleApiErrorResponse(error);

        // Clear any potentially cached tokens on error
        localStorage.removeItem(Token_Key);
        localStorage.removeItem(User_Refresh_Token);
    }
}

const postLoginRoute = async () => {
    const lastRoute = localStorage.getItem(Last_Route);

    var postLoginRoute = lastRoute ? JSON.parse(lastRoute) : { name: HomeRoute, params: {}, query: {}, };
    
    
    if (route.query.logoffMethod != ManualLogoffMethod) {
        const { previousRoute } = appContextStore;

        if(previousRoute && previousRoute.path !== "/" && previousRoute.name) {
            postLoginRoute = {
                name: previousRoute.name,
                params: previousRoute.params,
                query: previousRoute.query,
            };
        }
    }
    if(lastRoute == LogoffRoute) {
        router.replace({name: HomeRoute, params: {}});
    }
    else {
        router.replace(postLoginRoute);
    }
}

</script>

<template>
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
                            <div class="card card-custom">
                                <div class="card-header border-0 pt-7">
                                    <h3 class="card-title align-items-start flex-column">
                                        <span class="card-label fw-bolder text-dark">Log in</span>
                                    </h3>
                                </div>
                                <div class="card-body">
                                    <div class="form-group">
                                        <label>User Identifier:</label>
                                        <input type="text" class="form-control" v-model="userIdentifier" />
                                    </div>
                                    <div class="form-group">
                                        <label>Password:</label>
                                        <input type="password" class="form-control" v-model="password" />
                                    </div>
                                    <ButtonWithLoadingIndicator class="btn btn-primary"
                                        :icon="'fa-solid fa-right-to-bracket'" :label="'Log in'"
                                        :loading="loginInProgress" @click.prevent="login">
                                        Log in
                                    </ButtonWithLoadingIndicator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>