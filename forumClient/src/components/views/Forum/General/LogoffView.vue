<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import { useAppContextStore } from '@/stores/AppContextStore';
import { Token_Key, Last_Logged_In_User_Identifier, Last_Route } from '@/LocalStorage/keys';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import ErrorHandler from '@/Handlers/ErrorHandler';
import LoginService from '@/services/LoginService';
import { SetToken } from '@/http/AxiosClient';
import { useRoute } from 'vue-router';
import { Toast_UserLogoffSuccessful } from '@/components/prepopulatedToasts';
import { LoginRoute } from '@/router';

const appContextStore = useAppContextStore();

const route = useRoute();
const router = useRouter();

const loading = ref<boolean>(false);

const onLogoff = () => {
    localStorage.removeItem(Token_Key);
    localStorage.removeItem(Last_Route);
    localStorage.removeItem(Last_Logged_In_User_Identifier);
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