<script lang = "ts" setup>

const props = defineProps<{
    searchQuery: string,
    handleSearch: (query: string) => void
}>();

const emit = defineEmits(['update-search-query']);

const onInput = (event: Event) => {
    const target = event.target as HTMLInputElement;
    emit('update-search-query', target.value);
    props.handleSearch(target.value);
}

</script>

<template> 
    <div class="search-bar">
        <div class="input-group">
            <input type="text" class="form-control" placeholder="Search..." :value="searchQuery" @input="onInput">
            <div class="input-group-append">
                <span class="input-group-text">
                    <i class="flaticon2-search-1"></i>
                </span>
            </div>
        </div>
    </div>
</template>