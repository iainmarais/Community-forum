<script lang = "ts" setup>
import type { UpdateUserProfileRequest } from '@/Dto/UserInfo';
import { useUserProfileStore } from '@/stores/UserProfileStore';
import { onMounted, ref, watch, type PropType } from 'vue';
import { useToast } from 'vue-toastification';

const userProfileStore = useUserProfileStore();
const toast = useToast();

const props = defineProps({
    userId: String as PropType<string>,
    userProfileImageBase64: String as PropType<string>,
    userFirstname: String as PropType<string>,
    userLastname: String as PropType<string>,
});

const userProfileImageBase64 = ref<string>(props.userProfileImageBase64 || "");
const userFirstName = ref<string>(props.userFirstname || "");
const userLastName = ref<string>(props.userLastname || "");
const userId = ref<string>(props.userId || "");

const userProfileImageDescription = ref<string>("Current image");

const newUserProfileImageBase64 = ref<string>("");

const convertToBase64 = (file: File) => {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            resolve(reader.result);
        };
        reader.onerror = (error) => {
            reject(error);
            toast.error("Could not load image");
        };
    });
}

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
        const imgElement = document.getElementById("userProfileImage") as HTMLImageElement;
        imgElement.src = imageUrl;
    } catch (error) {
        toast.error("Could not load image");
    }
}

const handleFileChange = async (event:Event) => {
    const target = event.target as HTMLInputElement;
    if(target.files && target.files[0]) {
        try {
            const file = target.files![0];
            const base64 = await convertToBase64(file);
            newUserProfileImageBase64.value = base64 as string;
        }
        catch (error) {
            toast.error("Could not load image");
        }
    }
}

const closeModal = () => {
    $('#userProfileModal').modal("hide");
}

const updateUserProfile = () => {
    if (userFirstName.value == "" || userLastName.value == "") {
        toast.error("Please enter a first and last name");
        return;
    }
    if (newUserProfileImageBase64.value == "") {
        toast.error("Please select an image");
        return;
    }
    const request: UpdateUserProfileRequest = {
        userId: userId.value,
        userFirstname: userFirstName.value,
        userLastname: userLastName.value,
        userProfileImageBase64: newUserProfileImageBase64.value
    }
    userProfileStore.updateUserProfile(request);
    if(userProfileStore.result_updateUserProfileSuccess) {
        closeModal();
    }
}

watch(userProfileImageBase64, (newValue) =>{
    if(newValue != "") {
        displayImage(newValue);
    }
});

watch(props, newProps => {
    if(newProps.userId) {
        userId.value = newProps.userId;
    }
    if(newProps.userProfileImageBase64) {
        userProfileImageBase64.value = newProps.userProfileImageBase64;
        displayImage(newProps.userProfileImageBase64);
    }
    if(newProps.userFirstname) {
        userFirstName.value = newProps.userFirstname;
    }
    if(newProps.userLastname) {
        userLastName.value = newProps.userLastname;
    }
});

watch(newUserProfileImageBase64, newValue => {
    if(newValue != "") {
        userProfileImageDescription.value = "New image"; 
    }
    else {
        userProfileImageDescription.value = "Current image";
    }
});

onMounted(() => {
    if (userProfileImageBase64.value != "") {
        displayImage(userProfileImageBase64.value);
    }
});

</script>

<template>
    <div id="userProfileModal" class="modal fade stackable" role="dialog" aria-labelledby="staticBackdrop" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">User Profile</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <table class="table table-borderless table-sm">
                        <tr>
                            <td>First Name</td>
                            <td><input type="text" class="form-control" v-model="userFirstName" :placeholder="userFirstname"></td>
                        </tr>
                        <tr>
                            <td>Last Name</td>
                            <td><input type="text" class="form-control" v-model="userLastName" :placeholder="userLastname"></td>
                        </tr>
                        <tr>
                            <td>Profile Image</td>
                            <td><input type="file" class="form-control" @change="handleFileChange"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: flex; align-content: center; justify-content: center">
                                    <div class="polaroid-frame-classic">
                                        <img class="polaroid-image" id="userProfileImage" alt="User profile image">
                                        <div class="polaroid-caption">
                                            <p class="polaroid-caption-title">{{ userFirstName }} {{ userLastName }}</p>
                                            <p class="polaroid-caption-text">{{ userProfileImageDescription }}</p>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-danger" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" @click="updateUserProfile()">Update Profile</button>
                </div>
            </div>
        </div>
    </div>
</template>