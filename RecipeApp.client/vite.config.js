import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tsconfigPaths from 'vite-tsconfig-paths'

export default defineConfig({
  plugins: [react(), tsconfigPaths()],
  build: {
    rollupOptions: {
      // Force Rollup to print the real error details
      onwarn(warning, warn) {
        console.error("REAL ERROR DETAILS:", warning.message);
        warn(warning);
      }
    }
  }
})
