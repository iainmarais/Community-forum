import { createApp } from 'vue';
import { createPinia } from 'pinia';

import App from './App.vue'
import router from './router';

import Toast, {useToast, type PluginOptions} from 'vue-toastification';

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

import ConfigurationLoader from './config/ConfigurationLoader';
import type { ToastOptionsAndRequiredContent } from "vue-toastification/dist/types/types";

const app = createApp(App);
app.use(createPinia());
app.use(router);
app.use(Toast, toastOptions);
app.mount('#app');

app.config.errorHandler = function (err, vm, info) {
    console.error(err);
    if(err instanceof Error) {
        if(ConfigurationLoader.getConfig().envName === "local") {
            console.error(err.message);
        }
    }
}
