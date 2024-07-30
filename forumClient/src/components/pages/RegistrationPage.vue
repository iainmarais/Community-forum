<script lang = "ts" setup>
import { useRoute } from 'vue-router';

import { useRegistrationStore } from '@/stores/RegistrationStore';
import "@/assets/scss/pagestyles.scss";

const registrationStore = useRegistrationStore();


const username = "";
const emailAddress = "";
const password = "";
const retypePassword = "";

const route = useRoute();


const registerUser = () => {

    if(username.length == 0 || emailAddress.length == 0 || password.length == 0) {
        //Can't do jack without these details.
        return;
    }

    if(password != retypePassword) {
        //Passwords don't match. Dammit, I need a popup notifier of some sort here.
        return;
    }

    const request = {
        username: username,
        emailAddress: emailAddress,
        password: password,
        retypePassword: retypePassword
    }
    registrationStore.RegisterUser(request);
    //Do we trigger a login if the registration is successful? If so I probably need a datastore for this.
}

</script>

<template>
<div>
    <h1>Register</h1>
    <div class="row">
        <div class="col-md-4">
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
                <input type="password" class="form-control" id="password" v-model="password">
            </div>
            <div class="form-group">
                <label for="retypePassword">Retype Password</label>
                <input type="password" class="form-control" id="retypePassword" v-model="retypePassword">
            </div>
            <div class="form-group">
                <button class="btn btn-primary" @click="registerUser()">Register</button>
            </div>
        </div>
    </div>
</div>
</template>