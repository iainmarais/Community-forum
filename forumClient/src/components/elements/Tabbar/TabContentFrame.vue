<script lang = "ts" setup>
const props = defineProps({
    tabId: {
        type: String,
        required: true  
    },
    activeTabId: {
        type: String,
        required: true
    }
})

//Destructure this into properties that can be used by a parent component
const { tabId, activeTabId } = props;
</script>

<template>
    <div class="tab-pane fade" :class="{ 'show active': tabId === activeTabId }" :id="tabId">
        <slot>
            <!--Tab content will be placed here.-->
        </slot>
    </div>
</template>