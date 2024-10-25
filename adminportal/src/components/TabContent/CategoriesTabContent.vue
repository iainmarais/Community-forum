<script lang = "ts" setup>
import ButtonWithLoadingIndicator from '@/components/elements/ButtonWithLoadingIndicator.vue';
import { useContentManagementStore } from '@/stores/ContentManagementStore';
import LoadingIndicator from '@/components/LoadingIndicator.vue';
import { onMounted } from 'vue';

const contentManagementStore = useContentManagementStore();

const refresh = () => {
    contentManagementStore.getCategories();
}

onMounted(() => {
    contentManagementStore.getCategories();
})

</script>

<template>
    <div class="card card-custom">
        <div class="card-header">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">
                    Categories
                </span>
            </h3>
            <div class="card-toolbar">
                <!--Todo: add search function-->
                <ButtonWithLoadingIndicator :label="'Refresh'" :icon="'fas fa-sync'" class="btn btn-primary btn-sm" @click.prevent="refresh()">
                    Refresh
                </ButtonWithLoadingIndicator>
            </div>
        </div>
        <div class="card-body" v-if="!contentManagementStore.loading">
            <table class="table-sm table-borderless">
                <tr>
                    <th>
                        <td>Category name</td>
                        <td>Description</td>
                        <td>Number of Boards</td>
                    </th>
                </tr>
                <tr v-for="element in contentManagementStore.categories.rows">
                    <td> {{ element.category.categoryName ?? "N/A" }} </td>
                    <td> {{ element.category.categoryDescription ?? "N/A" }}</td>
                    <td> {{ element.boards.length ?? 0 }}</td>
                </tr>
            </table>
        </div>
        <div class="card-body" v-else>
            <LoadingIndicator :loading="true" />
        </div>
    </div>
</template>