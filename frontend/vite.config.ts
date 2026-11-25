/// <reference types="vitest/config" />
import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // The web app and the API share one origin (Architecture Overview):
    // in development the Vite dev server forwards /api requests to the ASP.NET Core API.
    proxy: {
      '/api': 'http://localhost:5188',
    },
  },
  test: {
    environment: 'jsdom',
    setupFiles: ['./src/test/setup.ts'],
  },
})
