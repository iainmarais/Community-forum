<script lang = "ts" setup>
import { ref, type PropType } from 'vue';

const props = defineProps({
    searchMode: String as PropType<string>,
    viewMode: String as PropType<string>,
    search: Function as PropType<(query: string) => void>
});

const searchQuery = ref<string>("");

const handleSearch = () => {
    if (props.search && searchQuery.value.trim() !== "") {
        props.search(searchQuery.value);   
    }
}

//If searchMode is set to "local", search must check against the store that contains the items to search.

</script>

<template>
    <div :class="{ 'search-bar': props.viewMode !== 'view', 'search-bar-page': props.viewMode === 'view'}">
        <div :class="{'search-input-group' : props.viewMode !== 'view', 'search-input-group-page' : props.viewMode === 'view'}">
            <input v-model="searchQuery" type="text" :class="{'search-input-form-control': props.viewMode !== 'view', 'search-input-form-control-page': props.viewMode === 'view'}" placeholder="Search">
            <div :class="{'search-input-group-append': props.viewMode !== 'view', 'search-input-group-append-page': props.viewMode === 'view'	}">
                <button :class="{' search-bar-button': props.viewMode !== 'view', 'search-bar-button-page': props.viewMode === 'view'}" class="btn navbar-search-button" type="button" @click="handleSearch">
                    <i class="fa fa-search search-bar-icon"></i>
                </button>
            </div>
        </div>
    </div>
</template>

<style lang = "scss" scoped>
//Global style for search:

//Searchbar
.search-bar {
    display: inline-flexbox;
    width: 100%;
    border-radius: 5px;
    background-color: white;
}

.search-bar-page {
    display: inline-flexbox;
    width: 100%;
    border-radius: 5px;
    background-color: white;
    border: 1px solid black;
}

//input
.search-input-group {
    display:flex;
    flex-direction: row;
    border-radius: 5px;
    font-size: 16px;
    border: none;
    outline: none;
    border-radius: 5px;
}

.search-input-group-page {
    display:flex;
    flex-direction: row;
    border-radius: 5px;
    font-size: 14px;
    border: none;
    outline: none;
    border-radius: 5px;
}


.search-input-form-control {
    font-size: 16px;
    padding: 5px 10px;
    border: none;
    outline: none;
    border-radius: 5px;
}

.search-input-form-control-page {
    font-size: 14px;
    padding: 5px 10px;
    border: none;
    outline: none;
    border-radius: 5px;
}


.search-input-group-append {
    font-size: 16px;
    border: none;
    outline: none;
    border-radius: 5px;
}

.search-input-group-append-page {
    font-size: 14px;
    border: none;
    outline: none;
    border-radius: 5px;
}

//button
.search-bar-button {
    background-color: navy;
    font-size: 16px;
    border: none;
    outline: none;
    border-radius: 0px;
    border-top-right-radius: 5px;
    border-bottom-right-radius: 5px;
    width: 60px;
}

.search-bar-button-page {
    background-color: #0BB783;
    font-size: 10px;
    border: none;
    outline: none;
    border-radius: 0px;
    border-top-right-radius: 5px;
    border-bottom-right-radius: 5px;
    width: 60px;
}

.search-bar-icon {
    color: white;
}
</style>