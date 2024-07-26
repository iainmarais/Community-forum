<script lang = "ts" setup> 
import { onMounted, ref, watch, type PropType } from 'vue';
import { useAppContextStore } from '@/stores/AppContextStore';
import { useDiscussionStore } from '@/stores/Discussions/DiscussionStore';
import type { PostFullInfo } from '@/Dto/app/PostInfo';

const discussionStore = useDiscussionStore();
const appContextStore = useAppContextStore();

const props = defineProps ({
    activeThreadId: String
});

const postContent = ref<string>("");
const message = ref<PostFullInfo>({} as PostFullInfo);

const saveMessage = () => {
    /* 
    Needed for posting:  
        Creator user id - handled by the backend, retrieved from the user context.
        Thread id - set by frontend on submit
        Post content - set by frontend on submit
    For replies:
        reply to message id - set by frontend on submit only on a reply. It is normally not set.
    Editing existing posts:
        Selected nessage id - should be set by the frontend and compared against the message id of the post to edit. They should be the same.
    
    Todo: build out.
    */
}

</script>

<template>
    <div class = "modal fade" id = "createNewPostModal" tabindex = "-1" role = "dialog" aria-labelledby = "createNewPostModal" aria-hidden = "true">
        <div class = "modal-dialog modal-xxl" role = "document">
            <div class = "modal-content">
                <div class = "modal-header">
                    <h5 class = "modal-title font-weight-bolder text-dark075 font-size-h5">Post Message</h5>
                    <button type = "button" class = "close" data-dismiss = "modal" aria-label = "Close">
                        <i aria-hidden = "true" class = "ki ki-close"></i>
                    </button>
                </div>
                <div class = "modal-body">
                    <div class = "form-group">
                        <label for = "postContent">Post Content</label>
                        <textarea class = "form-control" id = "postContent" v-model = "postContent"></textarea>
                    </div>
                </div>
                <div class = "modal-footer">
                    <button type = "button" class = "btn btn-outline-danger font-weight-bold" data-dismiss = "modal">Close</button>
                    <button type = "button" class = "btn btn-primary font-weight-bold" @click = "saveMessage()">Save</button>
                </div>
            </div>
        </div>
    </div>
</template>