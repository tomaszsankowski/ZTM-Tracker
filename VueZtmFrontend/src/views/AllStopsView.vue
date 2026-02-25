<template>
  <div class="min-h-screen bg-gray-900 p-4 text-white">
    <div class="container mx-auto">
      <div class="mb-8 rounded-lg bg-gray-800 p-6 shadow-lg ring-1 ring-white/10">
        <h2 class="mb-4 text-xl font-semibold text-white">All ZTM Stops</h2>
        <vue-good-table
          :columns="columns"
          :rows="stopsStore.allStops"
          :search-options="{ enabled: true }"
          :pagination-options="{ enabled: true }"
          theme="nocturnal"
        >
          <template #table-row="props">
            <span v-if="props.column.field == 'actions'">
              <span v-if="isStopAdded(props.row.stopId)" class="text-green-500 font-semibold">Added</span>
              <button v-else @click="openAddModal(props.row)" class="rounded bg-indigo-500 px-3 py-1 text-xs font-semibold text-white hover:bg-indigo-400">Add to My Stops</button>
            </span>
            <span v-else>
              {{ props.formattedRow[props.column.field] }}
            </span>
          </template>
        </vue-good-table>
      </div>
    </div>

    <StopModal
      :is-open="isModalOpen"
      :initial-name="selectedStopName"
      title="Add Stop"
      @close="isModalOpen = false"
      @save="saveStop"
    />
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useStopsStore } from '@/stores/stops';
import { VueGoodTable } from 'vue-good-table-next';
import 'vue-good-table-next/dist/vue-good-table-next.css';
import StopModal from '@/components/StopModal.vue';

const stopsStore = useStopsStore();

const columns = [
  { label: 'Stop ID', field: 'stopId' },
  { label: 'Name', field: 'stopName' },
  { label: 'Description', field: 'stopDesc' },
  { label: 'Actions', field: 'actions' },
];

const isModalOpen = ref(false);
const selectedStop = ref<any>(null);
const selectedStopName = ref('');

onMounted(async () => {
  await Promise.all([
    stopsStore.fetchAllStops(),
    stopsStore.fetchUserStops()
  ]);
});

const isStopAdded = (stopId: number) => {
  return stopsStore.stops.some((s: any) => s.stopId === stopId);
};

const openAddModal = (stop: any) => {
  selectedStop.value = stop;
  selectedStopName.value = stop.stopName || '';
  isModalOpen.value = true;
};

const saveStop = async (customName: string) => {
  if (selectedStop.value) {
    await stopsStore.addUserStop(selectedStop.value.stopId, customName);
    isModalOpen.value = false;
  }
};
</script>
