<script lang ="ts" setup>
import type { PropType } from 'vue';
import { RouterLink } from 'vue-router';


const props = defineProps({
    dstName: String,
    dstParams: Object as PropType<Record<string, any>>,
    iconClass: String,
    labelText: String,
    styleClass: {
        type: String,
        default: "btn-primary btn-sm"
    }
});


</script>

<template>
    <button class="btn" :class="styleClass">
        <RouterLink :to="{name: props.dstName, params: props.dstParams}" class="btn btn-primary btn-sm" :prepend-icon="props.iconClass">
            <div style = "margin-left: 5px;">
                <i :class="props.iconClass"></i>
                {{ props.labelText }}
            </div>
        </RouterLink>
    </button>
</template>