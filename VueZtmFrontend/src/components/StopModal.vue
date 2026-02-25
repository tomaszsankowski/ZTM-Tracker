<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
    <div class="w-full max-w-md rounded-lg bg-gray-800 p-6 shadow-xl ring-1 ring-white/10">
      <h3 class="mb-4 text-lg font-semibold text-white">{{ title }}</h3>
      
      <div class="mb-4">
        <label class="mb-2 block text-sm font-medium text-gray-300">Custom Name</label>
        <input 
          v-model="name" 
          type="text" 
          class="block w-full rounded-md bg-white/5 px-3 py-2 text-white outline-none ring-1 ring-white/10 focus:ring-2 focus:ring-indigo-500"
          placeholder="Enter custom name"
          @keyup.enter="save"
        />
      </div>

      <div class="flex justify-end gap-3">
        <button 
          @click="$emit('close')" 
          class="rounded-md px-4 py-2 text-sm font-semibold text-gray-300 hover:bg-white/5"
        >
          Cancel
        </button>
        <button 
          @click="save" 
          class="rounded-md bg-indigo-500 px-4 py-2 text-sm font-semibold text-white hover:bg-indigo-400"
        >
          Save
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';

const props = defineProps<{
  isOpen: boolean;
  initialName: string;
  title: string;
}>();

const emit = defineEmits(['close', 'save']);

const name = ref(props.initialName);

watch(() => props.initialName, (newVal) => {
  name.value = newVal;
});

const save = () => {
  emit('save', name.value);
};
</script>
