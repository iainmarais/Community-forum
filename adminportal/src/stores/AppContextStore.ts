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

        getAppState() {
            this.appLoading = true;
            AdminPortalService.getAdminPortalAppState().then(async response => {
                this.appLoading = false;
                toast.success("App state updated from server");
                if(!response.data.currentLoggedInUser) {
                    this.currentLoggedInUser = defaultState.currentLoggedInUser;
                }
                if(!response.data.adminPortalStats) {
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
            this.currentLoggedInUser = undefined;

            AxiosClient.ForceLogoff();
            this.previousRoute = undefined;
            router.replace({ name: 'login', query: { logoffReason: reason, logoffMethod: "manual" } });
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