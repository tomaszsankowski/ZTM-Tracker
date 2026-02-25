import { defineStore } from 'pinia';
import { ref } from 'vue';
import apiClient from '@/api/axios';
import { useNotificationStore } from './notification';

export const useStopsStore = defineStore('stops', () => {
  const stops = ref<any[]>([]);
  const allStops = ref<any[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);

  async function fetchUserStops() {
    loading.value = true;
    try {
      const response = await apiClient.get('/UserStops');
      stops.value = response.data;
    } catch (err: any) {
      error.value = err.message;
    } finally {
      loading.value = false;
    }
  }

  async function fetchAllStops() {
    loading.value = true;
    try {
      const response = await apiClient.get('/Ztm/stops');
      allStops.value = response.data;
    } catch (err: any) {
      error.value = err.message;
    } finally {
      loading.value = false;
    }
  }

  async function addUserStop(stopId: number, customName: string) {
    const notificationStore = useNotificationStore();
    try {
      await apiClient.post('/UserStops', { stopId, customName });
      await fetchUserStops();
      notificationStore.showNotification(`Stop added successfully!`, 'success');
    } catch (err: any) {
      error.value = err.message;
      throw err;
    }
  }

  async function removeUserStop(id: string) {
    const notificationStore = useNotificationStore();
    try {
      await apiClient.delete(`/UserStops/${id}`);
      await fetchUserStops();
      notificationStore.showNotification('Stop removed successfully!', 'success');
    } catch (err: any) {
      error.value = err.message;
      throw err;
    }
  }

  async function updateUserStop(id: string, stopId: number, customName: string) {
    const notificationStore = useNotificationStore();
    try {
      await apiClient.put(`/UserStops/${id}`, { stopId, customName });
      await fetchUserStops();
      notificationStore.showNotification('Stop updated successfully!', 'success');
    } catch (err: any) {
      error.value = err.message;
      throw err;
    }
  }

  return { stops, allStops, loading, error, fetchUserStops, fetchAllStops, addUserStop, removeUserStop, updateUserStop };
});
