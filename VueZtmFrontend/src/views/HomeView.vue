<template>
  <div class="min-h-screen bg-gray-900 p-4 text-white">
    <div class="container mx-auto">
      <div class="mb-8 flex items-center justify-between">
        <h1 class="text-3xl font-bold tracking-tight text-white">My Stops</h1>
      </div>

      <div class="mb-8 rounded-lg bg-gray-800 p-6 shadow-lg ring-1 ring-white/10">
        <h2 class="mb-4 text-xl font-semibold text-white">Add New Stop</h2>
        <div class="flex flex-col gap-4 sm:flex-row">
          <input v-model="newStopId" type="number" placeholder="Stop ID (e.g. 2019)" class="block w-full rounded-md bg-white/5 px-3 py-1.5 text-base text-white outline-1 -outline-offset-1 outline-white/10 placeholder:text-gray-500 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-500 sm:text-sm/6" />
          <input v-model="newStopName" type="text" placeholder="Stop Name (e.g. Miszewskiego)" class="block w-full rounded-md bg-white/5 px-3 py-1.5 text-base text-white outline-1 -outline-offset-1 outline-white/10 placeholder:text-gray-500 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-500 sm:text-sm/6" />
          <button @click="addStop" class="rounded-md bg-indigo-500 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-indigo-400 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-500">Add</button>
        </div>
      </div>

      <div class="rounded-lg bg-gray-800 p-6 shadow-lg ring-1 ring-white/10">
        <vue-good-table
          :columns="columns"
          :rows="stopsStore.stops"
          :search-options="{ enabled: true }"
          :pagination-options="{ enabled: true }"
          theme="nocturnal"
        >
          <template #table-row="props">
            <span v-if="props.column.field == 'actions'">
              <button @click="viewDelays(props.row.stopId)" class="mr-2 rounded bg-indigo-500 px-3 py-1 text-xs font-semibold text-white hover:bg-indigo-400">Delays</button>
              <button @click="openEditModal(props.row)" class="mr-2 rounded bg-yellow-600 px-3 py-1 text-xs font-semibold text-white hover:bg-yellow-500">Edit</button>
              <button @click="removeStop(props.row.id)" class="rounded bg-red-600 px-3 py-1 text-xs font-semibold text-white hover:bg-red-500">Delete</button>
            </span>
            <span v-else>
              {{ props.formattedRow[props.column.field] }}
            </span>
          </template>
        </vue-good-table>
      </div>

      <div v-if="loadingDelays" class="mt-8 text-center text-gray-400">Loading...</div>
      <div v-else-if="selectedStopDelays" class="mt-8 rounded-lg bg-gray-800 p-6 shadow-lg ring-1 ring-white/10">
        <DelaysTable :delays="selectedStopDelays" :stop-id="selectedStopId" />
      </div>
    </div>

    <StopModal
      :is-open="isModalOpen"
      :initial-name="selectedStopName"
      title="Edit Stop"
      @close="isModalOpen = false"
      @save="saveStop"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useStopsStore } from '@/stores/stops';
import { useNotificationStore } from '@/stores/notification';
import { useZtmData } from '@/composables/useZtmData';
import { VueGoodTable } from 'vue-good-table-next';
import 'vue-good-table-next/dist/vue-good-table-next.css';
import DelaysTable from '@/components/DelaysTable.vue';
import StopModal from '@/components/StopModal.vue';

const stopsStore = useStopsStore();
const { data: selectedStopDelays, loading: loadingDelays, fetchDelays } = useZtmData();

const newStopId = ref('');
const newStopName = ref('');
const selectedStopId = ref<number | null>(null);

const isModalOpen = ref(false);
const selectedStop = ref<any>(null);
const selectedStopName = ref('');

const columns = [
  { label: 'Stop ID', field: 'stopId' },
  { label: 'Name', field: 'customName' },
  { label: 'Actions', field: 'actions' },
];

onMounted(() => {
  stopsStore.fetchUserStops();
});

const openEditModal = (stop: any) => {
  selectedStop.value = stop;
  selectedStopName.value = stop.customName || '';
  isModalOpen.value = true;
};

const saveStop = async (customName: string) => {
  if (selectedStop.value) {
    await stopsStore.updateUserStop(selectedStop.value.id, selectedStop.value.stopId, customName);
    isModalOpen.value = false;
  }
};

const addStop = async () => {
  const notificationStore = useNotificationStore();

  if (!newStopId.value) {
    notificationStore.showNotification('Pole Stop ID jest wymagane.', 'error');
    return;
  }
  
  const id = parseInt(newStopId.value);
  if (isNaN(id)) {
    notificationStore.showNotification('Nieprawidłowe ID przystanku.', 'error');
    return;
  }

  if (id > 2147483647) {
    notificationStore.showNotification('ID przystanku jest zbyt duże.', 'error');
    return;
  }

  await stopsStore.addUserStop(id, newStopName.value);
  newStopId.value = '';
  newStopName.value = '';
};

const removeStop = async (id: string) => {
  await stopsStore.removeUserStop(id);
};

const viewDelays = async (stopId: number) => {
  selectedStopId.value = stopId;
  await fetchDelays(stopId);
};
</script>
