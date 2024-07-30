import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import { fileURLToPath, URL } from 'url';
export const hash = Math.floor(Math.random() * 90000) + 10000;
// https://vitejs.dev/config/
export default defineConfig({
    plugins: [vue()],
    css: {
        preprocessorOptions: {
            scss: {
                additionalData: `@import "./src/assets/scss/base.scss";`
            }
        }
    },
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url)),
        }
    },
    build: {
        rollupOptions: {
            output: {
                entryFileNames: `[name]` + hash + `.js`,
                chunkFileNames: `[name]` + hash + `.js`,
                assetFileNames: `[name]` + hash + `.[ext]`
            }
        }
    }
});
