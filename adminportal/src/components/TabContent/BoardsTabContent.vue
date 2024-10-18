<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';


const refresh = () => {
    //Todo: build out.
}

</script>

<template>
    <div class="card card-custom">
        <div class="card-header">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    Boards
                </span>
            </h3>
            <div class="card-toolbar">
                <!--Todo: add search function-->
                <ButtonWithLoadingIndicator :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body">
            <p>Coming soon</p>
        </div>
    </div>
</template>