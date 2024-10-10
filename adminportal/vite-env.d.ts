/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly Vite_ApiV1_BaseUrl: string;
    readonly Vite_ApiV2_BaseUrl: string;
    readonly Vite_EnvName: string;
    readonly env: ImportMetaEnv;
}

interface ImportMeta {
    readonly env: ImportMetaEnv
}