import { defineStore } from 'pinia';
import { ref } from 'vue';

export type NotificationType = 'success' | 'error' | 'info';

export const useNotificationStore = defineStore('notification', () => {
  const message = ref('');
  const type = ref<NotificationType>('info');
  const show = ref(false);
  let timeoutId: number | undefined;

  function showNotification(msg: string, notifType: NotificationType = 'info', duration = 3000) {
    message.value = msg;
    type.value = notifType;
    show.value = true;

    if (timeoutId) {
      clearTimeout(timeoutId);
    }

    timeoutId = setTimeout(() => {
      show.value = false;
    }, duration);
  }

  function hideNotification() {
    show.value = false;
    if (timeoutId) {
      clearTimeout(timeoutId);
    }
  }

  return { message, type, show, showNotification, hideNotification };
});
