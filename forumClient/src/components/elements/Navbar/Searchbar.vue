<script lang = "ts" setup>
import { useAppContextStore } from '@/stores/AppContextStore';
</script>

<template>
    <div class="search-bar">
        <div class="search-input-group">
            <input type="text" class="search-input-form-control" placeholder="Search">
            <div class="search-input-group-append">
                <button class="btn search-bar-button navbar-search-button" type="button">
                    <i class="fa fa-search"></i>
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

.search-input-form-control {
    font-size: 16px;
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
</style>