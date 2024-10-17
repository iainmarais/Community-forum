<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import LoadingIndicator from '@/components/LoadingIndicator.vue';

</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    User management
                </span>
            </h3>
            <div class="card-toolbar">
                <ButtonWithLoadingIndicator :label="'Refresh'" :icon="'fas fa-sync'"  class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="text-center">
                        <span>Coming soon...</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>