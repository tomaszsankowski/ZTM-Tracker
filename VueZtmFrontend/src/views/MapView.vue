<template>
  <div class="h-[calc(100vh-64px)] z-0 w-full">
    <l-map ref="map" v-model:zoom="zoom" :center="center" :use-global-leaflet="false">
      <l-tile-layer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        layer-type="base"
        name="OpenStreetMap"
      ></l-tile-layer>
      
      <l-marker 
        v-for="stop in validStops" 
        :key="stop.stopId" 
        :lat-lng="[stop.stopLat, stop.stopLon]"
      >
        <l-popup>
          <div class="text-gray-900">
            <h3 class="font-bold">{{ stop.stopName }}</h3>
            <p class="text-sm">ID: {{ stop.stopId }}</p>
            <button 
              @click="addStopDirectly(stop)"
              class="mt-2 rounded bg-indigo-500 px-3 py-1 text-xs text-white hover:bg-indigo-600"
            >
              Add to My Stops
            </button>
          </div>
        </l-popup>
      </l-marker>
    </l-map>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { LMap, LTileLayer, LMarker, LPopup } from '@vue-leaflet/vue-leaflet';
import { useStopsStore } from '@/stores/stops';

const stopsStore = useStopsStore();
const zoom = ref(12);
const center = ref([54.3520, 18.6466]);

const validStops = computed(() => {
  return stopsStore.allStops.filter(s => s.stopLat && s.stopLon);
});

onMounted(async () => {
  await stopsStore.fetchAllStops();
});

const addStopDirectly = async (stop: any) => {
  await stopsStore.addUserStop(stop.stopId, stop.stopName);
};
</script>
