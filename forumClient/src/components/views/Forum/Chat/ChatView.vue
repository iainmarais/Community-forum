<script lang = "ts" setup>
import { useRouter } from 'vue-router';
import ContactsAndChatsTabView from './ContactsAndChatsTabView.vue';

const router = useRouter();

const goBack = () => {
    //Return to last route. Leaving for now.
    router.go(-1);
}
</script>

<template>
    <div class="card card-custom">
        <div class="card-header border-0 pt-7">
            <h3 class="card-title align-items-start flex-column">
                <span class="card-label font-weight-bolder text-dark075 font-size-h5">Chat</span>
            </h3>
            <div class="card-toolbar">
                <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-arrow-left"></i>Back</button>
            </div>
        </div>
        <div class="card-body">
            <!--Layout should be with a tabbed chat/contact list on the right, the chat on the left, and in the same frame as the chat, the input box.-->
            <div class="row">
                <div class="col-md-9">
                    <!--Chat view box-->
                    <div class = "row">
                        <div class="card card-custom">
                            <div class="card-header border-0 pt-7">
                                <h3 class="card-title align-items-start flex-column">
                                    <span class="card-label font-weight-bolder text-dark075 font-size-h5">Chat view</span>
                                </h3>
                            </div>
                            <div class="card-body">
                                <span>Chat view - coming soon...</span>
                            </div>
                        </div> 
                    </div>
                    <!--Input box-->
                    <div class = "row">
                        <div class="card card-custom">
                            <div class="card-header border-0 pt-7">
                                <h3 class="card-title align-items-start flex-column">
                                    <span class="card-label font-weight-bolder text-dark075 font-size-h5">Input</span>
                                </h3>
                                <div class="card-toolbar">
                                    <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-arrow-right"></i>Send</button>
                                    <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-ban"></i>Clear</button>
                                </div>                                
                            </div>
                            <div class="card-body">
                                <input type="text" class="form-control" placeholder="Enter message">
                            </div>
                        </div>                                      
                    </div>
                </div>
                <!--Contact/chat list box-->
                <div class="col-md-3">
                    <div class="card card-custom">
                        <div class="card-header border-0 pt-7">
                            <h3 class="card-title align-items-start flex-column">
                                <ContactsAndChatsTabView />
                            </h3>
                            <div class="card-toolbar">
                                <!--tab navigation-->
                                
                            </div>
                        </div>
                        <div class="card-body">
                            <!--tab view-->
                        </div>
                        <span class = "text-muted">Coming soon...</span>
                    </div>
                </div>
            </div>
        </div>  
    </div>
</template>