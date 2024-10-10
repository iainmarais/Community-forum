<script lang = "ts" setup>
import { onMounted, onUnmounted, ref } from 'vue';

const isWidescreen = ref(window.innerWidth > 1200);

const handleResize = () => {
    isWidescreen.value = window.innerWidth > 1200;
}


onMounted(() => {
    window.addEventListener("resize", handleResize);
});

onUnmounted(() => {
    window.removeEventListener("resize", handleResize);
});

</script>

<template> 

    <div class = "navbar">
        <div class = "navbar-inner">
            <div class="navbar-brand">
                <a href = "#">Admin Portal</a>
            </div>
            <div class="navbar-content">
                <ul class="navbar-items">
                    <li class="navbar-item">
                        <a href = "#"><i class="fas fa-house"></i>Home</a>
                    </li>
                    <li class="navbar-item">
                        <a href = "#"><i class="fas fa-circle-question"></i>About</a>
                    </li>
                    <li class="navbar-item">
                        <a href = "#"><i class="fas fa-id-card"></i>Contact</a>
                    </li>
                </ul>
            </div>
        </div>
    </div>

</template>

<style>
@import "@/assets/scss/navbar.scss";
</style>