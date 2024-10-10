import { createApp } from 'vue';
import { createPinia } from 'pinia';
import { createVuetify } from 'vuetify';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import Toast, { useToast, type PluginOptions } from 'vue-toastification';
import { aliases, mdi } from 'vuetify/iconsets/mdi';

import 'vuetify/styles'; // Ensure you are using css-loader

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import 'font-awesome/css/font-awesome.min.css';
import '@mdi/font/css/materialdesignicons.css'; // Include Material Design Icons

import App from './App.vue';
import router from './router';
import ConfigurationLoader from './config/ConfigurationLoader';
import type { ToastOptionsAndRequiredContent } from "vue-toastification/dist/types/types";

const toastOptions = {
    filterBeforeCreate: (toast: ToastOptionsAndRequiredContent) => {
        if (toast.type === "success") {
            toast.timeout = 1500;
        }
        return toast;
    }
};

const vuetify = createVuetify({
    components,
    directives,
    icons: {
        defaultSet: 'mdi',
        aliases,
        sets: {
            mdi,
        },
    }
});



const app = createApp(App);
app.use(router);
app.use(createPinia());
app.use(vuetify);
app.use(Toast, toastOptions);

app.mount('#app');

app.config.errorHandler = function(err: any, vm: any, info: any) {
    console.error(err);
    if(err instanceof Error) { 
        if(ConfigurationLoader.getConfig().envName==="local") {
            console.error(err.message);
        }
    }
}
