import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      // Any request in your frontend starting with /api will be intercepted by Vite
      '/api': {
        target: 'https://localhost', // Your backend API URL or reverse proxy container
        changeOrigin: true,          // Changes the origin of the host header to the target URL
        secure: false,               // Bypasses self-signed SSL certificate checks in local dev
        
        // OPTIONAL: If your backend controller route is [Route("[controller]")] 
        // without an 'api' prefix in C#, uncomment the rewrite below:
        // rewrite: (path) => path.replace(/^\/api/, ''),
      },
    },
  },
})
