import { createApp } from 'vue';
import { createPinia } from 'pinia';
import { createVuetify } from 'vuetify';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import Toast, { useToast, type PluginOptions } from 'vue-toastification';
import { aliases, mdi } from 'vuetify/iconsets/mdi';

// Import jQuery and Bootstrap first
import "jquery/dist/jquery.min.js";
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

// Import other styles
import 'vuetify/styles'; // Ensure you are using css-loader
import "vue-toastification/dist/index.css";
import "./css/app.css";
import "./css/customstyles.scss";

// Import FontAwesome and Material Design Icons
import '@mdi/font/css/materialdesignicons.css'; // Include Material Design Icons
import '@fortawesome/fontawesome-free/css/all.css';

// Import local components and router
import App from './App.vue';
import router from './router';

// Import configuration and types
import ConfigurationLoader from './config/ConfigurationLoader';
import type { ToastOptionsAndRequiredContent } from "vue-toastification/dist/types/types";

// Configure toast options
const toastOptions = {
    filterBeforeCreate: (toast: ToastOptionsAndRequiredContent) => {
        if (toast.type === "success") {
            toast.timeout = 1500;
        }
        return toast;
    }
};

// Initialize jQuery for use in the global window object
import $ from 'jquery';
const windowA = window as any;
windowA.jQuery = windowA.$ = $;

// Create Vuetify instance
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

// Create Vue app instance and mount it
const app = createApp(App);
app.use(router);
app.use(createPinia());
app.use(vuetify);
app.use(Toast, toastOptions);
app.mount('#app');

// Error handling configuration
app.config.errorHandler = function(err: any, vm: any, info: any) {
    console.error(err);
    if(err instanceof Error) { 
        if(ConfigurationLoader.getConfig().envName==="local") {
            console.error(err.message);
        }
    }
}
