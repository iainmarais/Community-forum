<script lang="ts" setup>
import Swal from 'sweetalert2';

const useShowConfirm = (data: any) => {
    Swal.fire({
        title: data.title,
        text: data.text,
        icon: data.icon,
        confirmButtonText: data.confirmButtonText,
        showCancelButton: true        
    }).then((result) => {
        if (result.isConfirmed) {
            data.callback();
        }
    });
};

const useShowConfirmWithInput = (data: any) => {
    return Swal.fire(data);
};

</script>

<template>

</template>