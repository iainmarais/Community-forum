import { defineStore } from "pinia";
import AxiosClient from "@/http/AxiosClient";
import router, { LoginRoute, HomeRoute, ContentManagementRoute, LogoffRoute, UserManagementRoute, PermissionsManagement, SupportRequestsRoute } from "@/router";
import type { RouteParams, RouteQueryAndHash } from "vue-router";
import ErrorHandler from "@/Handlers/ErrorHandler";
import { useToast } from "vue-toastification";
import nprogress from "nprogress";
import { Last_Route } from "@/LocalStorage/keys";
import type { PermissionType } from "@/Dto/PermissionInfo";
import type { LoggedInUserInfo } from "@/Dto/LoggedInUserInfo";
import AdminPortalService from "@/Services/AdminPortalService";
import type { AdminPortalStats } from "@/Dto/AdminPortal/AdminPortalAppState";

const documentTitle = "Community Forum - Admin portal";
const toast = useToast();

export type FeatureId = "None";

const NavigationBar: NavbarItem[] = [
    {
        id: "compactMenu",
        type: "menu",
        label: "Menu",
        iconClass: "fa-solid  fa-bars",
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
                iconClass: "fa-solid fa-right-to-bracket",
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
                id: "usermgmt",
                type: "item",
                label: "User management",
                iconClass: "fa fa-user",
                routename: UserManagementRoute
            },
            {
                id: "permissionsmgmt",
                type: "item",
                label: "Permissions management",
                iconClass: "fa fa-key",
                routename: PermissionsManagement
            },
            {
                id: "supportrequests",
                type: "item",
                label: "Support requests",
                iconClass: "fa fa-question",
                routename: SupportRequestsRoute
            },  
            {
                id: "logoff",
                type: "item",
                label: "Log off",
                iconClass: "fa-solid fa-right-from-bracket",
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
        iconClass: "fa-solid  fa-right-to-bracket",
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
        id: "usermgmt",
        type: "item",
        label: "User management",
        iconClass: "fa fa-user",
        routename: UserManagementRoute
    },    
    {
        id: "permissionsmgmt",
        type: "item",
        label: "Permissions management",
        iconClass: "fa fa-key",
        routename: PermissionsManagement
    },
    {
        id: "supportrequests",
        type: "item",
        label: "Support requests",
        iconClass: "fa fa-question",
        routename: SupportRequestsRoute
    },     
    {
        id: "logoff",
        type: "item",
        label: "Log off",
        iconClass: "fa-solid fa-right-from-bracket",
        routename: LogoffRoute
    },       
];

type NavbarLinkItemId = "home" | "login" | "logoff" | "usermgmt" | "permissionsmgmt" | "contentmgmt" | "supportrequests" ;

export type NavbarLinkItem = {
    type: "item";
    id: NavbarLinkItemId;
    label: string;
    labelClass?: string;
    iconClass: string;
    routename: string;
    routeParams?: any;
    featureFlag?: FeatureId;
}

export type NavbarMenuItem = {
    type: "menu";
    id: string;
    label: string;
    labelClass?: string;
    iconClass: string;
    items: NavbarItem[];
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
    currentLoggedInUser?: LoggedInUserInfo;
    appStats: AdminPortalStats;
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
    currentLoggedInUser: {} as LoggedInUserInfo,
    appStats: {} as AdminPortalStats,
    navbar: [],
}

export const useAppContextStore = defineStore({
    id: "AppContextStore",
    state: () => (defaultState),
    getters: {
        loggedInUserFullName(): string {
            if(this.currentLoggedInUser){
                return this.currentLoggedInUser.userFirstname + " " + this.currentLoggedInUser.userLastname;
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
            if(!this.currentLoggedInUser) {
                return false;
            }
            if(permissions.length === 0) {
                return true;
            }
            return permissions.some(permission => permissions.includes(permission));
        },

        async getAppState() {
            this.appLoading = true;
            try {
                const response = await AdminPortalService.getAdminPortalAppState();
                
                // Update user state
                if (response.data.currentLoggedInUser) {
                    this.currentLoggedInUser = response.data.currentLoggedInUser;
                } else {
                    this.currentLoggedInUser = undefined;
                }
                
                // Update app stats
                if (response.data.adminPortalStats) {
                    this.appStats = response.data.adminPortalStats;
                } else {
                    this.appStats = {} as AdminPortalStats;
                }
                
                // Update navbar after state changes
                this.buildNavbar(!this.currentLoggedInUser);
                
                toast.success("App state updated from server");
            } catch (error: any) {
                toast.error(error.message);
                // Clear user state on error
                this.currentLoggedInUser = undefined;
                this.appStats = {} as AdminPortalStats;
            } finally {
                this.appLoading = false;
                this.onAppReady();
            }
        },

        onAppReady() {
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
                if(navitem.type === "item") {
                    if(isLoggedOff) {
                        if(navitem.id === "login") {
                            contextualNavbar.push(navitem);
                        }
                    }
                    else {
                        if(navitem.id !== "login") {
                            contextualNavbar.push(navitem);
                        }
                    }
                }
                else if(navitem.type === "menu") {
                    const submenuItems: NavbarItem[] = [];
                    for(const menuItem of navitem.items) {
                        if(menuItem.type === "item") {
                            if(isLoggedOff) {
                                if(menuItem.id === "login") {
                                    submenuItems.push(menuItem);
                                }
                            }
                            else {
                                if(menuItem.id !== "login") {
                                    submenuItems.push(menuItem);
                                }
                            }
                        }
                        else if(menuItem.type === "submenu") {
                            submenuItems.push(menuItem);
                        }
                    }

                    if(submenuItems.length > 0) {
                        contextualNavbar.push({
                            ...navitem,
                            items: submenuItems
                        });
                    }
                }
            }
            this.navbar = contextualNavbar;
        },
        async logoff(reason?: string) {
            try {
                // Clear user state first
                this.currentLoggedInUser = undefined;
                this.appStats = {} as AdminPortalStats;
                
                // Clear all tokens and cached data
                localStorage.removeItem("Token_Key");
                localStorage.removeItem("User_Refresh_Token");
                
                // Force client cleanup
                AxiosClient.ForceLogoff();
                this.previousRoute = undefined;
                
                // Rebuild navbar for logged out state
                this.buildNavbar(true);
                
                // Navigate to login page
                await router.replace({ 
                    name: 'login', 
                    query: { 
                        logoffReason: reason, 
                        logoffMethod: "manual",
                        timestamp: Date.now().toString() // Prevent caching
                    } 
                });
            } catch (error) {
                console.error('Error during logoff:', error);
                // Even if there's an error, ensure user is logged out
                router.replace({ name: 'login' });
            }
        }
    }
});

router.beforeEach((to, from) => {
    nprogress.start();
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