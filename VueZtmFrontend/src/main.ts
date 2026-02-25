import './assets/tailwind.css'
import 'leaflet/dist/leaflet.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import apiClient from './api/axios'
import { useAuthStore } from './stores/auth'
import { useNotificationStore } from './stores/notification'
import { delayColor } from './directives/delayColor'
import datePlugin from './plugins/datePlugin'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)
app.use(datePlugin)
app.directive('delay-color', delayColor)

apiClient.interceptors.request.use((config) => {
  const authStore = useAuthStore()
  if (authStore.token) {
    config.headers.Authorization = `Bearer ${authStore.token}`
  }
  return config
})

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    const notificationStore = useNotificationStore()
    let message = error.response?.data?.message || error.message || 'An unexpected error occurred'

    if (error.response?.data?.errors) {
      const errors = error.response.data.errors;
      const errorMessages = Object.values(errors).flat().join(', ');
      if (errorMessages) {
        message = errorMessages;
      }
    } else if (error.response?.data?.title) {
       message = error.response.data.title;
    }

    notificationStore.showNotification(message, 'error')
    return Promise.reject(error)
  }
)

app.mount('#app')
