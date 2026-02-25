<script setup lang="ts">
import { RouterView, RouterLink } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import NotificationToast from '@/components/NotificationToast.vue'

const authStore = useAuthStore()
</script>

<template>
  <div class="min-h-screen bg-gray-900 font-sans text-gray-100">
    <nav
      v-if="authStore.isAuthenticated"
      class="sticky top-0 z-50 border-b border-white/10 bg-gray-800"
    >
      <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div class="flex h-16 items-center justify-between">
          <div class="flex items-center">
            <div class="shrink-0">
              <img class="h-8 w-8" src="/favicon.ico" alt="ZTM Tracker" />
            </div>
            <div class="hidden md:block">
              <div class="ml-10 flex items-baseline space-x-4">
                <RouterLink
                  to="/"
                  class="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                  active-class="bg-gray-900 text-white"
                  >My Stops</RouterLink
                >
                <RouterLink
                  to="/stops"
                  class="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                  active-class="bg-gray-900 text-white"
                  >All Stops</RouterLink
                >
                <RouterLink
                  to="/map"
                  class="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                  active-class="bg-gray-900 text-white"
                  >Map</RouterLink
                >
              </div>
            </div>
          </div>
          <div class="hidden md:block">
            <div class="flex items-center gap-4">
              <span v-if="authStore.user" class="text-gray-300 font-medium"
                >Hello, {{ authStore.user.login }}!</span
              >
              <button
                @click="authStore.logout()"
                class="rounded bg-red-600 px-4 py-2 text-sm font-semibold hover:bg-red-500 transition-colors"
              >
                Logout
              </button>
            </div>
          </div>
        </div>
      </div>
    </nav>
    <main>
      <NotificationToast />
      <RouterView />
    </main>
  </div>
</template>
