<script lang = "ts" setup>

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import 'bootstrap-icons/font/bootstrap-icons.css';
import * as bootstrap from 'bootstrap';
import { Tooltip } from 'bootstrap';
import '@/assets/plugins/global/plugins.bundle.js?url';
// import "@/assets/js/scripts.bundle.js?url";

import { RouterView } from 'vue-router';
import { onMounted } from 'vue';

onMounted(() => {
    const dropdownElementList = [].slice.call(document.querySelectorAll('.dropdown-toggle'));
    dropdownElementList.forEach(dropdownToggleEl => {
        new bootstrap.Dropdown(dropdownToggleEl);
    });
    var defaultAllowList = Tooltip.Default as any;
});

</script>

<template>
    <div>
        <RouterView />
    </div>
</template>

<style>
@import "@/assets/css/style.bundle.css";
@import "@/assets/plugins/global/plugins.bundle.css";
@import "assets/base.css";
</style>