/// <reference types="vite/client" />

declare module 'vue' {
  interface ComponentCustomProperties {
    $formatDate: (dateString: string) => string;
  }
}

export {}