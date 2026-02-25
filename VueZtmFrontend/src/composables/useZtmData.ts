import { ref } from 'vue';
import apiClient from '@/api/axios';

export function useZtmData() {
  const data = ref<any>(null);
  const loading = ref(false);
  const error = ref<string | null>(null);

  const fetchDelays = async (stopId: number) => {
    loading.value = true;
    error.value = null;
    try {
      const response = await apiClient.get(`/Ztm/delays/${stopId}`);
      data.value = response.data;
    } catch (err: any) {
      error.value = err.message || 'Failed to fetch data';
    } finally {
      loading.value = false;
    }
  };

  return { data, loading, error, fetchDelays };
}
