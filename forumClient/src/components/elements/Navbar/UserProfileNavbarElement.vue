<script lang = "ts" setup>
import { useToast } from 'vue-toastification';
import UserProfileModal from '@/components/modals/UserProfileModal.vue';
import type { LoggedInUserInfo } from '@/Dto/app/ForumAppState';
import { onMounted, ref, watch, type PropType } from 'vue';
import { useAppContextStore } from '@/stores/AppContextStore';

const appContextStore = useAppContextStore();

const openUserProfileModal = () => {
    $("#userProfileModal").modal("show");
}

const toast = useToast();
const loggedInUserInfo = ref<LoggedInUserInfo|null>();

const userProfileImageBase64 = ref<string>("");

const convertFromBase64 = (imageDataBase64: string) => {
    return new Promise<Blob>((resolve, reject) => {
        try {
            const base64Data = imageDataBase64.split(",")[1];
            const binaryString = atob(base64Data);
            const binaryLength = binaryString.length;
            const bytes = new Uint8Array(binaryLength);
            for(var i = 0; i < binaryLength; i++) {
                bytes[i] = binaryString.charCodeAt(i);
            }
            //Reconstruct the image from the bytes
            const blob = new Blob([bytes], { type: 'image/jpeg' });
            resolve(blob);
        } catch (error) {
            reject(error);
            toast.error("Could not load image");
        }
    });
}

const displayImage = async (imageDataBase64: string) => {
    try {
        const blob = await convertFromBase64(imageDataBase64);
        const imageUrl= URL.createObjectURL(blob);
        const imgElement = document.getElementById("navbarUserProfileImage") as HTMLImageElement;
        imgElement.src = imageUrl;
    } catch (error) {
        toast.error("Could not load image");
    }
}

watch(() => appContextStore.loggedInUser, (newUser) => {
    if(newUser) {
        loggedInUserInfo.value = newUser;
        if(newUser.userProfileImageBase64) {
            userProfileImageBase64.value = newUser.userProfileImageBase64;
            displayImage(newUser.userProfileImageBase64);
        }
    }
});

onMounted(() => {
    if(appContextStore.loggedInUser != null) {
        loggedInUserInfo.value = appContextStore.loggedInUser;
        if(appContextStore.loggedInUser.userProfileImageBase64) {
            userProfileImageBase64.value = appContextStore.loggedInUser.userProfileImageBase64;
            displayImage(appContextStore.loggedInUser.userProfileImageBase64);
        }
    }
});

</script>

<template>
    <div class="user-profile-navbar-element">
        <img v-if="userProfileImageBase64" class="navbar-userprofile-image" id="navbarUserProfileImage" alt="User profile image" @click="openUserProfileModal">
        <img v-else src="@/assets/media/png/placeholderImage300x300.png" class="navbar-userprofile-image" id="navbarUserProfileImage" alt="placeholder for image" @click="openUserProfileModal">
    </div>
<UserProfileModal :userId = "loggedInUserInfo?.userId"/>
</template>

<style lang="scss" scoped>

.navbar-userprofile-image {
    width:40px;
    border-radius: 10%;
}
</style>