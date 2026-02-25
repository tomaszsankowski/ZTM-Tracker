import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import apiClient from '@/api/axios';
import router from '@/router';
import { useNotificationStore } from './notification';

export const useAuthStore = defineStore('auth', () => {
  const token = ref(localStorage.getItem('token') || '');
  const user = ref(JSON.parse(localStorage.getItem('user') || 'null'));

  const isAuthenticated = computed(() => !!token.value);

  function setToken(newToken: string) {
    token.value = newToken;
    localStorage.setItem('token', newToken);
  }

  function setUser(newUser: any) {
    user.value = newUser;
    localStorage.setItem('user', JSON.stringify(newUser));
  }

  async function login(credentials: any) {
    const notificationStore = useNotificationStore();
    try {
      const response = await apiClient.post('/Auth/login', credentials);
      const receivedToken = response.data.accessToken || response.data.token || response.data; 
      setToken(receivedToken);
      setUser({ login: credentials.login });
      notificationStore.showNotification('Logged in successfully!', 'success');
      router.push('/');
    } catch (error) {
      console.error('Login failed', error);
      throw error;
    }
  }

  async function register(credentials: any) {
     const notificationStore = useNotificationStore();
     try {
      await apiClient.post('/Auth/register', credentials);
      notificationStore.showNotification('Registration successful! Please login.', 'success');
      router.push('/login');
    } catch (error) {
      console.error('Registration failed', error);
      throw error;
    }
  }

  function logout() {
    const notificationStore = useNotificationStore();
    token.value = '';
    user.value = null;
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    notificationStore.showNotification('Logged out successfully.', 'info');
    router.push('/login');
  }

  return { token, user, isAuthenticated, login, register, logout };
});
