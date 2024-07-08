import { defineStore } from "pinia";


type Breadcrumb = {
    text: string;
    route: {
        name: string;
        params?: any;
    }
}

type MainPageStore = {
    loading: boolean,
    pageTitle: string,
    breadcrumbs: Breadcrumb[]
}

const defaultState: MainPageStore = {
    loading: false,
    pageTitle: "",
    breadcrumbs: []
}

export const useMainPageStore = defineStore({
    id: "MainPageStore",
    state: () => (defaultState),
    getters: {},
    actions: {
        setBreadcrumbs(breadcrumbs: Breadcrumb[]) {
            this.breadcrumbs = breadcrumbs;
    },
    setPageTitle(title: string) {
        document.title = `Forum - ${title}`;
        this.pageTitle = title;
    },
}
});