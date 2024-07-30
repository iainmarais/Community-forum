<script lang = "ts" setup>
import { useRoute, useRouter } from 'vue-router';
import { useDiscussionStore } from '@/stores/Discussions/DiscussionStore';
import { onMounted, ref, watch, type PropType } from 'vue';
import type { ThreadBasicInfo, ThreadFullInfo } from '@/Dto/app/ThreadInfo';
import { useToast } from 'vue-toastification';
import LoadingIndicator from '@/components/elements/LoadingIndicator.vue';
import CreateNewPostModal from '@/components/modals/CreateNewPostModal.vue';
import type { UserBasicInfo } from '@/Dto/UserInfo';
import UserService from '@/services/UserService';
import DateUtils from '@/components/utils/DateUtils';
import { useAppContextStore } from '@/stores/AppContextStore';

const discussionStore = useDiscussionStore();
const appContextStore = useAppContextStore();

const toast = useToast();
const router = useRouter();
const createdByUser = ref<UserBasicInfo>();

const selectedPostId = ref<string>("");
const route = useRoute();
const threadId = ref<string>(route.params.threadId as string || "");

const thread = ref<ThreadFullInfo>({
    thread: {
        threadName: "",
    } as ThreadBasicInfo,
} as ThreadFullInfo);

const postReply = () => {
    if(!appContextStore.loggedInUser) {
        toast.error("You must be logged in to post a reply");
        router.push({name: "login"});
    }
    $("#createNewPostModal").modal("show");
}

const replyToPostId = (postId: string) => {
    if(!appContextStore.loggedInUser) {
        toast.error("You must be logged in to post a reply");
        router.push({name: "login"});
    }
    selectedPostId.value = postId;
    $("#createNewPostModal").modal("show");
}

const handlePostCreated = () => {
    discussionStore.getThreadFullInfo(threadId.value);
}

const goBack = () => {
    router.go(-1);
}

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
}

onMounted(() => {
    if (threadId.value) {
        discussionStore.getThreadFullInfo(threadId.value);
    }
});

watch(() => discussionStore.thread, (newThread) => {
    if (newThread == null) return;
    thread.value = newThread;
});

watch(() => route.params.threadId, (newThreadId) => {
    threadId.value = newThreadId as string || "";
    if(threadId.value === "home") {
        router.push({name: "home"}); 
    }
});

watch(() => appContextStore.loggedInUser, newValue => {
    if (!newValue) {
        toast.error("You must be logged in to view this page.");
        router.push({name: "login"});
    }
});

/*
    Todo, per similarly structured view component:
        Check if the user's logged in, if not, redirect to login page. This should/will help avoid inconsistencies in the user experience.
        Last thing I want is someone bellyaching about their post being lost because their session ended.
*/
</script>

<template>
<div class="card card-custom">
    <div class="card-header border-0 pt-7" v-if="!discussionStore.loading_discussion">
        <h3 class="card-title align-items-start flex-column">
            <span class="card-label font-weight-bolder text-dark075 font-size-h5">Discussion: {{ thread.thread.threadName }}</span>
        </h3>
        <div class="card-toolbar"> 
            <!--How does one add some padding between the elements of the toolbar?-->
            <button class="btn btn-primary btn-sm m-1" @click="postReply"><i class="fas fa-plus"></i>Post reply</button>
            <button class="btn btn-primary btn-sm m-1" @click="goBack"><i class="fas fa-arrow-left"></i>Back</button>
        </div>
    </div>
    <div class="card-body">
        <div class="card card-custom" v-for="post in thread.posts">
            <div class="card-header border-0 pt-7">
                <h3 class="card-title align-items-start flex-column">
                    <div class="d-flex align-items-center">
                        <div class="symbol symbol-50 flex-shrink-0 mr-4">
                            <!-- <div class="symbol-label" :style="`background-image: url(${getUserInfo(post.createdByUserId)?.profileImageUrl})`"></div> -->
                            <i class="fas fa-user" style="font-size: 30px"></i>
                        </div>
                        <span class="card-label font-weight-bolder text-dark075 font-size-h5">{{ getUserInfo(post.createdByUserId)?.username}}</span>
                    </div>    
                </h3>
                <div class="card-toolbar">
                    <button class="btn btn-outline-primary btn-sm m-1" @click="replyToPostId(post.postId)"><i class="fas fa-reply"></i>Reply</button>
                    <button class="btn btn-outline-danger btn-sm m-1" @click=""><i class="fas fa-exclamation"></i>Report post</button>
                </div>
            </div>
            <div class="card-body">
                <p>{{ post.postContent }}</p>
            </div>
            <div class="card-footer">
                <p>{{ DateUtils.formatDate(post.createdDate) }}</p>
            </div>
        </div>
    </div>
    
    <LoadingIndicator :loading="discussionStore.loading_discussion" />
</div>
<CreateNewPostModal :active-thread-id=threadId :reply-to-post-id=selectedPostId @post-created="handlePostCreated()"/>
</template>