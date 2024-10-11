/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_API_V1_BASE_URL: string;
    readonly VITE_API_V2_BASE_URL: string;
    readonly VITE_ENV_NAME: string;
    readonly env: ImportMetaEnv;
}

interface ImportMeta {
    readonly env: ImportMetaEnv
}