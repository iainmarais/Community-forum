<script lang = "ts" setup>
import { useAppContextStore } from '@/stores/AppContextStore';
import { onMounted, ref, watch, type PropType } from 'vue';
import { useToast } from 'vue-toastification';
import UserProfileModal from '@/components/modals/UserProfileModal.vue';

const openUserProfileModal = () => {
    $("#userProfileModal").modal("show");
}

const appContextStore = useAppContextStore();
const toast = useToast();

const userProfileImageBase64 = ref<string>("");
const userFirstName = ref<string>("");
const userLastName = ref<string>("");
const imageData = ref<string>(""); 

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

onMounted(() => {
    if (appContextStore.loggedInUser) {
        userFirstName.value = appContextStore.loggedInUser.userFirstname || "";
        userLastName.value = appContextStore.loggedInUser.userLastname || "";
        userProfileImageBase64.value = appContextStore.loggedInUser.userProfileImageBase64 || "";
    }
});

watch(userProfileImageBase64, (newValue) =>{
    if(newValue != "") {
        displayImage(newValue);
    }
});

</script>

<template>
    <div class="user-profile-navbar-element">
        <img v-if="userProfileImageBase64" class="navbar-userprofile-image" id="navbarUserProfileImage" alt="User profile image" @click="openUserProfileModal">
        <img v-else src="@/assets/media/png/placeholderImage300x300.png" class="navbar-userprofile-image" id="navbarUserProfileImage" alt="placeholder for image" @click="openUserProfileModal">
    </div>
<UserProfileModal :user-firstname="userFirstName" :user-lastname="userLastName" :user-id="appContextStore.loggedInUser?.userId" :user-profile-image-base64="userProfileImageBase64" />
</template>

<style lang="scss" scoped>

.navbar-userprofile-image {
    width:40px;
    border-radius: 10%;
}
</style>