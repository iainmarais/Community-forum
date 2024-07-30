<script lang = "ts" setup>

import bootstrap, { Tooltip } from 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

import '@/assets/plugins/global/plugins.bundle.js?url';
// import "@/assets/js/scripts.bundle.js?url";

import { RouterView } from 'vue-router';
import { onMounted } from 'vue';

onMounted(() => {
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