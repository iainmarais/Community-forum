<script lang = "ts" setup>
import { VBtn } from 'vuetify/lib/components/index.mjs';
import type { RouteLocationRaw } from 'vue-router';
import { type PropType } from 'vue';

const props = defineProps({
    dstName: String,
    dstParams: Object as PropType<Record<string, any>>,
    iconClass: String,
    labelText: String,
});

</script>

<template>
    <VBtn :to="{name: props.dstName, params: props.dstParams}" class="navbar-button" :prepend-icon="props.iconClass">
        {{ props.labelText }}
    </VBtn>
</template>

<style scoped lang = "scss">
$navy-dark: darken(navy, 16%);
$navy-light: darken(navy, 6%);
$primary-color: #0c038a;
$primary-hover: #0056b3;
$primary-active: #003f7f;
$text-color: #fff;
$font-family: Arial, Helvetica, sans-serif;
$border-radius: 5px;


.navbar-button {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100px;
    height: 40px;
    border-radius: $border-radius;
    background-color: $primary-color;
    color: $text-color;
    font-family: $font-family;
    font-size: 12px;
    text-decoration: none;
    text-transform: capitalize;
    cursor: pointer;
    transition: background-color 0.3s ease;
    &:hover {
        background-color: $primary-hover;
    }
    &:active {
        background-color: $primary-active;
    }
}

.navbar-button-sm {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100px;
    height: 30px;
    border-radius: $border-radius;
    background-color: $primary-color;
    color: $text-color;
    font-family: $font-family;
    font-size: 12px;
    text-decoration: none;
    text-transform: capitalize;
    cursor: pointer;
    transition: background-color 0.3s ease;
    &:hover {
        background-color: $primary-hover;
    }
    &:active {
        background-color: $primary-active;
    }
}

</style>