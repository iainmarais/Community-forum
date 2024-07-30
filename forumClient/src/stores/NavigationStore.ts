import { defineStore } from "pinia";

type Breadcrumb = {
    text: string;
    route: {
        name: string;
        params?: any;
    }
}

type NavigationStoreState = {
    isNavOpen: boolean,
    breadcrumbs: Breadcrumb[],
}

export const useNavigationStore = defineStore({
    id: "NavigationStore",
    state: (): NavigationStoreState => ({
        isNavOpen: false,
        breadcrumbs: []
    }),
    getters: {

    },
    actions: {
        setBreadcrumbs(breadcrumbs: Breadcrumb[]) {
            this.breadcrumbs = breadcrumbs;
        },
    }
});