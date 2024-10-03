<script lang ="ts" setup>
import type { UserBasicInfo } from '@/Dto/UserInfo';
import type { GalleryItemBasicInfo } from '@/Dto/app/GalleryItemInfo';
import UserService from '@/services/UserService';
import { ref, watch, type PropType } from 'vue';

const props = defineProps({
    item: Object as PropType<GalleryItemBasicInfo>,
});

const item = ref(props.item);
const createdByUser = ref<UserBasicInfo>();

const getUserInfo = (userId: string) => {
    if (!createdByUser.value) {
        UserService.getUserById(userId).then((response) => {
            createdByUser.value = response.data;
        });
    }
    return createdByUser.value;
}


watch(() => props.item, (newItem) => {
    item.value = newItem;
});

</script>

<template>
    <div class="modal fade" id="infoModal" tabindex="-1" role="dialog" aria-labelledby="infoModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="infoModalLabel">Gallery item info: {{ props.item?.galleryItem.galleryItemName }} </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <!--Add more details here-->
                <div class="modal-body" v-if="props.item">
                    <p>Uploaded by {{ getUserInfo(props.item.galleryItem.createdByUserId)?.user.userFirstname }} {{ getUserInfo(props.item.galleryItem.createdByUserId)?.user.userLastname }}</p>
                </div>
                <div class ="modal-body" v-else>
                    <p>Could not load info.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</template>