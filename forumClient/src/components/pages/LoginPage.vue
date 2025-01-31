<script lang = "ts" setup>
import { Last_Logged_In_User_Identifier, Last_Route, Token_Key, User_Refresh_Token } from '@/LocalStorage/keys';
import { SetToken } from '@/http/AxiosClient';
import router, { LoginRoute, MainRoute, LogoffRoute, RegisterRoute, HomeRoute } from '@/router';
import LoginService from '@/services/LoginService';
import { useAppContextStore, type NavbarItem } from '@/stores/AppContextStore';
import {onMounted, ref, watch } from 'vue';
import ButtonWithLoadingIndicator from '../elements/ButtonWithLoadingIndicator.vue';
import { useRoute } from 'vue-router';
import ErrorHandler from '@/Handlers/ErrorHandler';
import { Toast_UserLoginSuccessful } from '../prepopulatedToasts';
import { useMainPageStore } from '@/stores/MainPageStore';
import "@/assets/scss/pagestyles.scss";
import Navbar from '../elements/Navbar.vue';
import type { UserLoginRequest } from '../../Dto/UserLoginRequest';
import { useToast } from 'vue-toastification';

const toast = useToast();

//Add additional login methods here.
type LoginMethod = "identifier_password";

const mainPageStore = useMainPageStore();
const password = ref<string>("");
const identifier = ref<string>("");

const appContextStore = useAppContextStore();

const loading = ref<boolean>(false);

const loginMethod = ref<LoginMethod>("identifier_password");
const loginInProgress = ref<boolean>(false);

const route = useRoute();

watch(identifier, () => {
    if(localStorage.getItem(Last_Logged_In_User_Identifier)) {
        localStorage.removeItem(Last_Logged_In_User_Identifier);
    }
});

onMounted(() => {
    $(".modal-backdrop").remove();
    const LastLoggedInIdentifier = localStorage.getItem(Last_Logged_In_User_Identifier);
    if(LastLoggedInIdentifier) {
        identifier.value = LastLoggedInIdentifier;
    }
    document.title ="Log in";
    if(route.params.logoffReason){
        console.log(route.params.logoffReason);
    }
    appContextStore.buildNavbar(true);
    const storedToken = localStorage.getItem(Token_Key);
    if(storedToken) {
        loading.value = true;
        SetToken(storedToken);
        postLoginRoute();
    }
    setTimeout(() => {
        if(loginMethod.value="identifier_password") {
            if(identifier.value) {
                document.getElementById("userIdentifierInput")?.focus();
            }
            else {
                document.getElementById("passwordInput")?.focus();
            }
        }
    }, 50);
});

const login = () => {
    const request:UserLoginRequest = {
        userIdentifier: identifier.value,
        password: password.value,
        //Hardcoded this here since in this case the user is logging in via the forum. It should be inferred based on the value set in the server-side DTO.
        userContext: "forum"
    }
    loginInProgress.value = true;
    LoginService.LoginUser(request).then(response => {
        if(response.data) {
            SetToken(response.data.accessToken);
            localStorage.setItem(Token_Key, response.data.accessToken);
            localStorage.setItem(Last_Logged_In_User_Identifier, request.userIdentifier);
            localStorage.setItem(User_Refresh_Token, response.data.userRefreshToken);
            loginInProgress.value = false;
            postLoginRoute();
            Toast_UserLoginSuccessful(response.data.userProfile.user.username);
        } 
        else {
            //Something went wrong while logging in or the payload returned by the server is invalid.
            toast.error("Could not log in. Please try again later.");
            loginInProgress.value = false;
        }
    }, error => {
        ErrorHandler.handleApiErrorResponse(error);
        loginInProgress.value = false;
    });
    
}

const postLoginRoute = () => {
    //Route to the login page.
    var postLoginRoute = {
        name: HomeRoute,
        params: {},
        query: {},
    }
    const lastRoute = localStorage.getItem(Last_Route); //Get the last route from localStorage if not login or logoff.
    
    if(lastRoute) {
        const lastRouteJson = JSON.parse(lastRoute);
        postLoginRoute = lastRouteJson;  
    }

    if(route.query.logoffMethod != "manual") {
        if(appContextStore.previousRoute) {
            if(appContextStore.previousRoute.path != "/") {
                if(appContextStore.previousRoute.name) {
                    postLoginRoute.name = appContextStore.previousRoute.name;
                    postLoginRoute.params = appContextStore.previousRoute.params;
                    postLoginRoute.query = appContextStore.previousRoute.query;
                }
            }
        }
    }
    if(lastRoute == LogoffRoute) {
        router.replace({name: HomeRoute, params: {}});
    }
    router.replace(postLoginRoute);
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
                                <div class="card-body p-0">
                                    <div class="row d-flex align-items-center justify-content-center">
                                        <div class="col-lg-6">
                                            <div class="p-5">
                                                <div class="mb-5">
                                                    <h3 class="font-weight-bolder text-dark">Welcome to the forum</h3>
                                                    <p class="text-muted">Please log in to your account</p>
                                                </div>
                                                <div class=form-group>
                                                    <label class="font-size-h6 font-weight-bolder text-dark">Username</label>
                                                    <input id="userIdentifierInput" class="form-control form-control-solid h-auto py-5 px-6" type="text"
                                                        name="username" placeholder="Username" v-model="identifier" />
                                                </div>
                                                <div class="form-group">
                                                    <label class="font-size-h6 font-weight-bolder text-dark">Password</label>
                                                    <input id="passwordInput" class="form-control form-control-solid h-auto py-5 px-6" type="password"
                                                        name="password" placeholder="Password" v-model="password" />
                                                </div>
                                                <div class="form-group">
                                                    <ButtonWithLoadingIndicator :icon="'fas fa-sign-in-alt'" :loading="loginInProgress" :label="'Log in'" @click.prevent="login()">
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
                </div>
            </div>
        </div>
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