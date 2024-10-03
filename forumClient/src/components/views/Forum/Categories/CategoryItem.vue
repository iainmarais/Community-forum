<script lang = "ts" setup>
import type { CategoryBasicInfo } from '@/Dto/app/CategoryInfo';
import { ref, watch, type PropType } from 'vue';
import { useToast } from 'vue-toastification';
import { useRouter } from 'vue-router';

const toast = useToast();
const router = useRouter();
const props = defineProps({
    category: Object as PropType<CategoryBasicInfo>
});

const categoryName = ref<string>("");

const viewCategory = (categoryId: string) => {
    router.push({ name: "ViewCategory", params: { categoryId: categoryId } });
    toast.info("Viewing category: " + getCategoryName());
}

const getCategoryName = () => {
    return props.category?.category.categoryName ?? "";
}

watch (() => props.category, (newCategory) => {
    if (newCategory) {
        categoryName.value = newCategory.category.categoryName;
    }
});

</script>

<template>
    <table class="table table-borderless table-sm">
        <tbody v-if="props.category" @click.prevent=viewCategory(props.category.category.categoryId)>
            <tr class="d-flex forum-element">
                <td>
                    <div class="d-flex align-items-center">
                        <div class="symbol symbol-50px me-5">
                            <span class="symbol-label bg-light">
                                <i class="fas fa-folder" style="font-size: 30px"></i>
                            </span>
                        </div>
                        <div class="ml-3">
                            <div class="text-dark font-weight-bolder text-hover-primary mb-1 font-size-lg">
                                {{ props.category.category.categoryName }}
                            </div>
                            <div>
                                <span class="text-muted font-weight-bold text-muted d-block">
                                    {{ props.category.category.categoryDescription }}
                                </span>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</template>