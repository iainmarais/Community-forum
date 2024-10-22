<script lang = "ts" setup>

const props = defineProps({
    currentPageNumber: Number,
    totalPages: Number,
    totalRows: Number
});
</script>

<template>
    <div class="page-selector">
        <span>Page {{ currentPageNumber }} of {{ totalPages }}</span>
        <div class="page-selector__buttons">
            <button class="btn btn-sm btn-primary" :disabled="currentPageNumber === 1" @click="$emit('previous-page')"><i class="fas fa-angle-left"></i> Previous</button>
            <!--show the last two pages if available-->

            <button v-for="i in totalPages" :key="i" class="btn btn-sm btn-primary" :disabled="currentPageNumber === i" @click="$emit('go-to-page', i)">{{ i }}</button>
            <!--show the next two pages if available-->
            
            <button class="btn btn-sm btn-primary" :disabled="currentPageNumber === totalPages" @click="$emit('next-page')"><i class="fas fa-angle-right"></i>Next</button>
        </div>
    </div>
</template>