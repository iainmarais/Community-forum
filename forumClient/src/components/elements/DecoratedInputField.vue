<script lang="ts" setup>
    const props = defineProps({
        modelValue: String, // For v-model support
        label: String,
        placeholder: String,
        type: { type: String, default: "password" },
        doPasswordsMatch: { type: Boolean, default: false },
        name: String, // To be used in forms
        required: Boolean // If required is needed for validation
    });

    const emit = defineEmits(['update:modelValue']); // Emit an update for v-model

    // Emit the input event to support v-model
    const onInput = (event: Event) => {
        const target = event.target as HTMLInputElement;
        emit('update:modelValue', target.value);
    };
</script>

<template>
        <!-- Input field, bind value and emit input event for v-model -->
        <input
            :type="type"
            class="form-control"
            :placeholder="placeholder"
            :name="name"
            :required="required"
            :value="modelValue"
            @input="onInput"
        />

        <!-- Visual feedback icon placed next to the input field -->
        <span v-if="type === 'password'" class="input-icon">
            <icon v-if="doPasswordsMatch" name="fa-solid fa-check" class="text-success"></icon>
            <icon v-else name="fa-solid fa-xmark" class="text-danger"></icon>
        </span>
</template>

<style scoped>
.form-group {
    position: relative;
}

.input-icon {
    position: absolute;
    right: 10px;
    top: 50%;
    transform: translateY(-50%);
    font-size: 1.2rem;
    pointer-events: none;
}
</style>
``
