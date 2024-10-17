<script lang = "ts" setup>
import { Last_Logged_In_User_Identifier, Last_Route, Token_Key, User_Refresh_Token } from '@/LocalStorage/keys';
import router, { HomeRoute, LoginRoute, LogoffRoute } from '@/router';

import { onMounted, ref, watch } from 'vue';

import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';

const loading = ref<boolean>(false);

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
                        <ButtonWithLoadingIndicator :label="'Log off'" :icon="'fa-solid fa-right-from-bracket'" :loading="loading" @click.prevent = "onLogoff()">Log off</ButtonWithLoadingIndicator>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>