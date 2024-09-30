import { createApp } from 'vue';
import { createPinia } from 'pinia';
import { createVuetify } from 'vuetify';
import "vuetify/styles";
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import { aliases, mdi } from 'vuetify/iconsets/mdi';

import App from './App.vue'
import router from './router';

import Toast, {useToast, type PluginOptions} from 'vue-toastification';

const vuetify = createVuetify({
    components,
    directives,
    icons: {
        defaultSet: 'mdi',
        aliases,
        sets: {
            mdi,
        }
    }
});

const toastOptions = {
    filterBeforeCreate: (toast: ToastOptionsAndRequiredContent) => {
        if(toast.type == "success") {
            toast.timeout = 1500;
        }
        return toast;
    }
};

import $ from 'jquery';
const windowA = window as any;
windowA.jQuery = windowA.$ = $;

import "vue-toastification/dist/index.css";
import "jquery/dist/jquery.min.js";
import "./css/app.css";
import "./css/customstyles.scss";

import ConfigurationLoader from './config/ConfigurationLoader';
import type { ToastOptionsAndRequiredContent } from "vue-toastification/dist/types/types";

const app = createApp(App);
app.use(createPinia());
app.use(vuetify);
app.use(router);
app.use(Toast, toastOptions);
app.mount('#app');

app.config.errorHandler = function (err: any, vm: any, info: any) {
    console.error(err);
    if(err instanceof Error) {
        if(ConfigurationLoader.getConfig().envName === "local") {
            console.error(err.message);
        }
    }
}
