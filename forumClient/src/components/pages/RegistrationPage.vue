<script lang = "ts" setup>
import { useRoute } from 'vue-router';

import { useRegistrationStore } from '@/stores/RegistrationStore';
import { useMainPageStore } from '@/stores/MainPageStore';
import "@/assets/scss/pagestyles.scss";
import ButtonWithLoadingIndicator from '../elements/ButtonWithLoadingIndicator.vue';

import Navbar from '../elements/Navbar.vue';
import DecoratedInputField from '../elements/DecoratedInputField.vue';
import router, { LoginRoute } from '@/router';
import { ref } from 'vue';
import { useToast } from 'vue-toastification';

const registrationStore = useRegistrationStore();
const mainPageStore = useMainPageStore();
const toast = useToast();

const username = ref("");
const emailAddress = ref("");
const password = ref("");
const retypePassword = ref("");

const route = useRoute();


const registerUser = () => {

    if(username.value.length == 0 || emailAddress.value.length == 0 || password.value.length == 0) {
        //Can't do jack without these details.
        toast.error("Please enter a username, email address and password");
        return;
    }

    if(password.value.trim() != retypePassword.value.trim()) {
        //Passwords don't match
        toast.error("Passwords don't match");
        return;
    }

    const request = {
        username: username.value,
        emailAddress: emailAddress.value,
        password: password.value,
        retypePassword: retypePassword.value
    }
    registrationStore.RegisterUser(request);
    //Push the login route
    router.replace({name: LoginRoute, params: {}});
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
                                                    <p class="text-muted">User Registration</p>
                                                </div>                
                                                <div class="form-group">
                                                    <label for="username">Username</label>
                                                    <input type="text" class="form-control" id="username" v-model="username">
                                                </div>
                                                <div class="form-group">
                                                    <label for="emailAddress">Email Address</label>
                                                    <input type="text" class="form-control" id="emailAddress" v-model="emailAddress">
                                                </div>
                                                <div class="form-group">
                                                    <label for="password">Password</label>
                                                    <DecoratedInputField type="password" class="form-control" id="password" v-model="password" />
                                                </div>
                                                <div class="form-group">
                                                    <label for="retypePassword">Retype Password</label>
                                                    <!--How to show an indicator if the passwords don't match-->
                                                    <DecoratedInputField type="password" class="form-control" id="retypePassword" v-model="retypePassword" />
                                                </div>
                                                <div class="form-group">
                                                    <!--Prevent this from executing its function more than once, somehow-->
                                                    <ButtonWithLoadingIndicator :loading="registrationStore.loading_RegistrationInProgress" :icon="'fas fa-check'" :label="'Register'" @click.prevent="registerUser()" id="registerButton">Register</ButtonWithLoadingIndicator>
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