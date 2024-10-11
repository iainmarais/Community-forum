<script lang = "ts" setup>
import { Toast_UserLoginSuccessful } from '@/components/prepopulatedToasts';
import type { AdminLoginRequest } from '@/Dto/AdminPortal/AdminLoginRequest';
import ErrorHandler from '@/Handlers/ErrorHandler';
import { SetToken } from '@/http/AxiosClient';
import { Last_Logged_In_User_Identifier, Last_Route, Token_Key, User_Refresh_Token } from '@/LocalStorage/keys';
import router, { HomeRoute, LoginRoute, LogoffRoute } from '@/router';
import AdminPortalService from '@/Services/AdminPortalService';
import { useAppContextStore } from '@/stores/AppContextStore';
import { useMainPageStore } from '@/stores/MainPageStore';
import { onMounted, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
import { useToast } from 'vue-toastification';
import NavigationBar from '@/components/Navbar.vue';

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

const onLogoff = () => {
    localStorage.removeItem(Token_Key);
    localStorage.removeItem(Last_Route);
    localStorage.removeItem(Last_Logged_In_User_Identifier);
    localStorage.removeItem(User_Refresh_Token);
    router.push({name: LoginRoute});
}

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Log off</span>
            </h3>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="text-center">
                        <span>Are you sure you want to log off?</span>
                    </div>
                    <div class="form-group">
                        <ButtonWithLoadingIndicator :label="'Log off'" :icon="'fas fa-sign-out-alt'" :loading="loading" @click = "onLogoff()">Log off</ButtonWithLoadingIndicator>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>