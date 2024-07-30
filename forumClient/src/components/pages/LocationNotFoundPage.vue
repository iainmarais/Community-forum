<script lang = "ts" setup>
import { useRouter } from 'vue-router';
import { HomeRoute } from '@/router';
import "@/assets/scss/pagestyles.scss";

const router = useRouter();

const goBack = () => {
    router.go(-1);
}

</script>

<template>
    <div class="card card-custom">
        <div class="card-header">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Page Not Found</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm" @click="goBack()"><i class="fas fa-arrow-left"></i>Back</button>
            </div>
        </div>
        <div class="card-body">
            <p>The page you are looking for does not exist.</p>
            <p>Please check the URL and try again, or go back to the home page.</p>
        </div>
    </div>
</template>