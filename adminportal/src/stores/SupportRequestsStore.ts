import type { SupportRequest } from "@/Dto/AdminPortal/SupportRequest"
import { defineStore } from "pinia"

type SupportRequestsStore = {
    requests: SupportRequest[]
}

const defaultState: SupportRequestsStore = {
    requests: [] as SupportRequest[]
}

export const useSupportRequestsStore = defineStore({
    id: "SupportRequestsStore",
    state: () => defaultState,
    getters: {},
    actions: {}
})