<script lang = "ts" setup> 
import { onMounted, ref, watch, type PropType } from 'vue';
import { useAppContextStore } from '@/stores/AppContextStore';
import { useDiscussionStore } from '@/stores/Discussions/DiscussionStore';
import type { CreatePostRequest, PostFullInfo } from '@/Dto/app/PostInfo';

const discussionStore = useDiscussionStore();
const appContextStore = useAppContextStore();

const props = defineProps ({
    activeThreadId: String,
    replyToPostId: String
});

const emit = defineEmits(['postCreated']);

const postContent = ref<string>("");
const replyToPostId = ref<string>("");
const isFirstPost = ref<boolean>(false);

const getThreadPostCount = (threadId: string) => {
    return discussionStore.thread?.posts?.filter((post) => post.post.threadId === threadId).length ?? 0;
}

const postReply = () => {
    if(postContent.value == "") {
        return;
    }
    if(getThreadPostCount(props.activeThreadId!) == 0) {
        isFirstPost.value = true;   
    }

    const request: CreatePostRequest = {
        threadId: props.activeThreadId!,
        postContent: postContent.value,
        isFirstPost: isFirstPost.value,
        replyToPostId: replyToPostId.value,
        createdByUserId: appContextStore.loggedInUser!.userId
    }

    discussionStore.createPost(request);
}
watch (() => props.replyToPostId, (newReplyToPostId) => {
    replyToPostId.value = newReplyToPostId!;
})

watch(() => discussionStore.result_createPostSuccess, (newResult) => {
    if(newResult) {
        closeModal();
    }
})

const closeModal = () => {
    $("#createNewPostModal").modal("hide");
    emit('postCreated');
}

</script>

<template>
    <div class = "modal fade" id = "createNewPostModal" tabindex = "-1" role = "dialog" aria-labelledby = "createNewPostModal" aria-hidden = "true">
        <div class = "modal-dialog modal-xxl" role = "document">
            <div class = "modal-content">
                <div class = "modal-header">
                    <h5 class = "modal-title font-weight-bolder text-dark075 font-size-h5" v-if="props.replyToPostId == ''">Create New Post</h5><h5 class = "modal-title font-weight-bolder text-dark075 font-size-h5" v-if="props.replyToPostId != ''">Post Reply</h5>
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
                    <button type = "button" class = "btn btn-primary font-weight-bold" @click = "postReply()">Post</button>
                </div>
            </div>
        </div>
    </div>
</template>