<script lang = "ts" setup>
import { ref } from 'vue';


const tabs = ref([
    {id: "chats", name: "Chats", active: false},
    {id: "contacts", name: "Contacts", active: true}
]);

</script>

<template>
    <div>
        <!--Tab bar -->
        <div class="card card-custom">
            <div class="card-header border-0 pt-7">
                <h3 class="card-title align-items-start flex-column">
                    <span class="card-label font-weight-bolder text-dark075 font-size-h5">Contacts and chats</span>
                </h3>
                <div class="card-toolbar">
                    <ul v-for="(tab, index) in tabs" class="nav nav-pills nav-pills-sm nav-dark-75">
                        <li class="nav-item">
                            <a class="nav-link py-2 px-4" 
                            :class="{active: index === 0}" 
                            :id="`#${tab.id}-tab`"
                            data-toggle="tab" 
                            href="`#${tab.id}`" 
                            role="tab"
                            :aria-controls="tab.id"
                            :aria-selected="tab.active">{{ tab.name }}</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="card-body">
                <!--tab view-->
            </div>
            <span class = "text-muted">Coming soon...</span>
        </div>
        <!--Tab content -->
    </div>
</template>