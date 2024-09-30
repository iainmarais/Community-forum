<script lang = "ts" setup>

const props = defineProps({
    tabId: String,
    tabIcon: String,
    tabName: String,
    isActive: Boolean
    //Add a prop for associated content? Not sure yet if I will need this.
});    

const emit = defineEmits(['tabSelected']); // Emit an update for v-model

//Destructure this into properties that can be used by a parent component
const { tabId, tabIcon, tabName, isActive } = props;
</script>

<template>
    <li class="nav-item">
        <a class="nav-link" data-toggle="tab" :class="{'active': isActive}" :href='`#${tabId}`' @click="emit('tabSelected', tabId)">
            <i :class="tabIcon"></i>
            {{ tabName }}
        </a>
    </li>
</template>