import { defineStore } from "pinia";
import AxiosClient from "@/http/AxiosClient";
import router, { LoginRoute, HomeRoute, ContentManagementRoute, LogoffRoute } from "@/router";
import type { RouteParams, RouteQueryAndHash } from "vue-router";

import ErrorHandler from "@/Handlers/ErrorHandler";

import { useToast } from "vue-toastification";
import NProgress from "NProgress";
import { Last_Route } from "@/LocalStorage/keys";
import type { PermissionType } from "@/Dto/PermissionInfo";
import type { LoggedInUserInfo } from "@/Dto/LoggedInUserInfo";
import AdminPortalService from "@/Services/AdminPortalService";
import type { ForumAppStats } from "@/Dto/AdminPortal/AdminPortalAppState";

const documentTitle = "Community Forum - Admin portal";
const toast = useToast();

export type FeatureId = "None";

const NavigationBar: NavbarItem[] = [
    {
        id: "compactMenu",
        type: "menu",
        label: "Menu",
        iconClass: "fas fa-bars",
        items: [
            {
                id: "home",
                type: "item",
                label: "Home",
                iconClass: "fas fa-home",
                routename: HomeRoute
            },
            {
                id: "login",
                type: "item",
                label: "Log in",
                iconClass: "fas fa-sign-in-alt",
                routename: LoginRoute
            },
            {
                id: "contentmgmt",
                type: "item",
                label: "Content management",
                iconClass: "fa fa-bars",
                routename: ContentManagementRoute
            },
            {
                id: "logoff",
                type: "item",
                label: "Log off",
                iconClass: "fas fa-sign-out-alt",
                routename: LogoffRoute
            },
        ]
    },
    {
        id: "home",
        type: "item",
        label: "Home",
        iconClass: "fas fa-home",
        routename: HomeRoute
    },
    {
        id: "login",
        type: "item",
        label: "Log in",
        iconClass: "fas fa-sign-in-alt",
        routename: LoginRoute
    },
    {
        id: "contentmgmt",
        type: "item",
        label: "Content management",
        iconClass: "fa fa-bars",
        routename: ContentManagementRoute
    }, 
    {
        id: "logoff",
        type: "item",
        label: "Log off",
        iconClass: "fas fa-sign-out-alt",
        routename: LogoffRoute
    },       
];

type NavbarLinkItemId = "home" | "login" | "logoff" | "contentmgmt";

export type NavbarLinkItem = {
    type: "item",
    id: NavbarLinkItemId;
    label: string;
    labelClass?: string;
    iconClass: string;
    routename: string;
    routeParams?: any;
    //Not yet used
    //permissions: PermissionType[]
    featureFlag?: FeatureId;
}

export type NavbarMenuItem = {
    type: "menu",
    id: string;
    label: string;
    labelClass?: string;
    iconClass: string;
    items: (NavbarLinkItem[] | NavbarSubmenuItem[]);
}

export type NavbarSubmenuItem = {
    type: "submenu";
    id: string;
    label: string;
    labelClass?: string;
    iconClass: string;
    items: NavbarLinkItem[];

}

export type NavbarItem = NavbarMenuItem | NavbarLinkItem | NavbarSubmenuItem;

type AppContextState = {
    clientName: string;
    appLoading: boolean;
    loggedInUser?: LoggedInUserInfo;
    appStats: ForumAppStats;
    navbar: NavbarItem[];
    previousRoute?: {
        path: string,
        name: string,
        query: RouteQueryAndHash,
        params: RouteParams
    }
}

const defaultState: AppContextState = {
    clientName: "",
    appLoading: true,
    loggedInUser: {} as LoggedInUserInfo,
    appStats: {} as ForumAppStats,
    navbar: [],
}

export const useAppContextStore = defineStore({
    id: "AppContextStore",
    state: () => (defaultState),
    getters: {
        loggedInUserFullName(): string {
            if(this.loggedInUser){
                return this.loggedInUser.userFirstname + " " + this.loggedInUser.userLastname;
            }
            else {
                return "Anonymous";
            }
        }
    },
    actions: {
        setClientname (clientname: string) {
            this.clientName = clientname;
        },

        getHasPermission(permissions: PermissionType[]) {
            if(!this.loggedInUser) {
                return false;
            }
            if(permissions.length === 0) {
                return true;
            }
            return permissions.some(permission => permissions.includes(permission));
        },

        getAppState() {
            this.appLoading = true;
            AdminPortalService.getAdminPortalAppState().then(async response => {
                this.appLoading = false;
                toast.success("App state updated from server");
                if(!response.data.loggedInUser) {
                    this.loggedInUser = defaultState.loggedInUser;
                }
                if(!response.data.forumAppStats) {
                    this.appStats = defaultState.appStats;
                }
                try {
                    //If something goes wrong 
                }
                catch (error: any) {
                    toast.error(error.message);
                }
                finally {
                    this.onAppReady();
                }
            }, error => {
                if(error.response?.status === 401) {
                    ErrorHandler.handleApiErrorResponse(error);
                    this.logoff("Authentication failed. Please log in again.");
                }
            });
        },

        onAppReady() {
            this.buildNavbar();
            this.appLoading = false;
            console.log(`App loading: ${this.appLoading}`)
        },

        //Use this to update nav bar elements like new posts, new threads, etc.
        getNavbarItemState(navbarItem: NavbarItem) {
            switch (navbarItem.id) {
                //Todo: add items that need to be polled
                default:
                    return undefined;
            }
        },
        buildNavbar(isLoggedOff: boolean = false) {
            const contextualNavbar: NavbarItem[] = [];
            for(const navitem of NavigationBar) {
                if(navitem.type==="item") {
                    if(!isLoggedOff && navitem.id === "login") {
                        continue;
                    }
                    else if(isLoggedOff && navitem.id === "logoff") {
                        continue;
                    }
                    else {
                        contextualNavbar.push(navitem);
                    }
                }

                if(navitem.type==="menu") {
                    const submenuItems: (NavbarLinkItem | NavbarSubmenuItem)[] = [];
                    for(const menuItem of navitem.items as (NavbarLinkItem | NavbarSubmenuItem)[]) {
                        if(menuItem.type==="item") {
                            submenuItems.push(menuItem);
                        }
                        const subSubmenuItems: NavbarLinkItem[] = [];
                        if(menuItem.type=="submenu") {
                            for(const subMenuItem of menuItem.items) {
                                subSubmenuItems.push(subMenuItem);
                            }
                            if(subSubmenuItems.length > 0) {
                                const subSubmenu: NavbarSubmenuItem = {
                                    ...menuItem,
                                    items: subSubmenuItems
                                };
                                submenuItems.push(subSubmenu);
                            }
                        }
                    }
                    if(submenuItems.length > 0) {
                        contextualNavbar.push({
                            ...navitem,
                            items: submenuItems as NavbarLinkItem[]
                        });
                    }
                }
            }
            this.navbar = contextualNavbar;
        },
        logoff(reason?: string) {
            this.loggedInUser = undefined;

            AxiosClient.ForceLogoff();
            this.previousRoute = undefined;
            router.replace({ name: 'login', query: { logoffReason: reason, logoffMethod: "manual" } });
        }
    }
});

router.beforeEach((to, from) => {
    document.title = `${documentTitle} - ${to.name as string}`;
    if(from && from.name) {
        useAppContextStore().previousRoute = {
            path: from.path,
            name: from.name as string,
            query: from.query,
            params: from.params
        }
    }
});