<template>
  <div v-if="delays">
    <h2 class="mb-4 text-xl font-semibold text-white">Live Delays for Stop {{ stopId }}</h2>
    <table class="min-w-full table-auto text-left text-sm text-gray-300">
      <thead class="bg-white/5 text-white">
        <tr>
          <th class="px-4 py-2">Route</th>
          <th class="px-4 py-2">Headsign</th>
          <th class="px-4 py-2">Estimated</th> <th class="px-4 py-2">Delay</th>
        </tr>
      </thead>
      <tbody class="divide-y divide-white/10">
        <tr v-for="delay in delays.departures" :key="delay.id">
          <td class="px-4 py-2 font-bold text-white">{{ delay.routeId }}</td>
          <td class="px-4 py-2">{{ delay.headsign }}</td>
          
          <td class="px-4 py-2 text-gray-100">
            {{ formatTime(delay.estimatedTime) }}
          </td>

          <td class="px-4 py-2" v-delay-color="delay.delayInSeconds">
            {{ formatDelay(delay.delayInSeconds) }}
          </td>
        </tr>
      </tbody>
    </table>
    <div class="mt-2 text-right text-xs text-gray-500">Last update: {{ $formatDate(delays.lastUpdate) }}</div>
  </div>
</template>

<script setup lang="ts">
import { format, parseISO } from 'date-fns';

defineProps<{
  delays: any;
  stopId: number | null;
}>();

const formatTime = (isoDate: string | null) => {
  if (!isoDate) return '-';
  return format(parseISO(isoDate), 'HH:mm');
};

const formatDelay = (seconds: number | null) => {
  if (seconds === null) return '-';
  const mins = Math.floor(Math.abs(seconds) / 60);
  return seconds < 0 ? `-${mins} min` : `+${mins} min`;
};
</script>