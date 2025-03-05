import { defineStore } from "pinia";
import AxiosClient from "@/http/AxiosClient";
import router, { LoginRoute, MainRoute, AdminRoute, LogoffRoute, RegisterRoute, HomeRoute, ChatRoute, GalleryRoute, SearchRoute } from "@/router";
import type { RouteParams, RouteQueryAndHash } from "vue-router";
import type { ForumStats, LoggedInUserInfo } from "@/Dto/app/ForumAppState";
import ForumService from "@/services/ForumService";
import type { TopicBasicInfo } from "@/Dto/app/TopicInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import type { PermissionType } from "@/Dto/PermissionInfo";
import { useToast } from "vue-toastification";
import NProgress from "nprogress";
import { Last_Route } from "@/LocalStorage/keys";

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
                id: "logoff",
                type: "item",
                label: "Log off",
                iconClass: "fas fa-sign-out-alt",
                routename: LogoffRoute
            },
            {
                id: "register",
                type: "item",
                label: "Register",
                iconClass: "fas fa-user-plus",
                routename: RegisterRoute
            },
            {
                id: "chat",
                type: "item",
                label: "Chat",
                iconClass: "fas fa-comments",
                routename: ChatRoute
            },
            {
                id: "gallery",
                type: "item",
                label: "Gallery",
                iconClass: "fas fa-images",
                routename: GalleryRoute
            },
            {
                id: "search",
                type: "item",
                label: "Search",
                iconClass: "fas fa-search",
                routename: SearchRoute
            },
            {
                id: "admin",
                type: "item",
                label: "Administration",
                iconClass: "fas fa-user-shield",
                routename: AdminRoute
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
        id: "logoff",
        type: "item",
        label: "Log off",
        iconClass: "fas fa-sign-out-alt",
        routename: LogoffRoute
    },
    {
        id: "register",
        type: "item",
        label: "Register",
        iconClass: "fas fa-user-plus",
        routename: RegisterRoute
    },
    {
        id: "chat",
        type: "item",
        label: "Chat",
        iconClass: "fas fa-comments",
        routename: ChatRoute
    },
    {
        id: "gallery",
        type: "item",
        label: "Gallery",
        iconClass: "fas fa-images",
        routename: GalleryRoute
    },
    {
        id: "search",
        type: "item",
        label: "Search",
        iconClass: "fas fa-search",
        routename: SearchRoute
    },
    {
        id: "admin",
        type: "item",
        label: "Admin",
        iconClass: "fas fa-user-shield",
        routename: AdminRoute
    },
];

type NavbarLinkItemId = "home" | "admin" | "login" | "register" | "logoff" | "chat" | "gallery" | "search";

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
    navbar: NavbarItem[];
    pollers: any[];
    topics: TopicBasicInfo[];
    forumStats: ForumStats;
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
    navbar: [],
    pollers: [],
    topics: [],
    forumStats: {
        totalPosts: 0,
        totalUsers: 0,
        totalThreads: 0,
        totalTopics: 0,
        mostActiveUsers: [],
        popularTopics: 0
    } 
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
        startPollers() {
            this.stopPollers();
            const getStatePoll = setInterval(() => {
                ForumService.getForumAppState().then(async response => {
                    if(response.data) {
                        this.forumStats = response.data.forumStats;
                    }
                }, error => {
                    if(error.response?.status === 401) {
                        this.stopPollers(); 
                    }
                });
            }, 30000);
            this.pollers.push(getStatePoll);
        },

        stopPollers() {
            this.pollers.forEach(poller => {
                clearInterval(poller);
            });
            this.pollers = [];
        },
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
            ForumService.getForumAppState().then(async response => {
                //Note to self:
                //Restructure this method to get details like app config and the logged-in user's details.
                //Stats should be done by the pollers.
                if(!response.data) {
                    this.forumStats = defaultState.forumStats;
                }
                else {
                    this.forumStats = response.data.forumStats;
                }
                if(!response.data) {
                    this.loggedInUser = defaultState.loggedInUser;
                }
                else {
                    //At this point, data will not be null
                    this.loggedInUser = response.data.loggedInUser!;
                }
                try {
                    // This might still be built using flagsmith or some other feature flagging lib.
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
        getPublicAppState() {
            //Todo: Build this out. It will be necessary for enabling guest mode. It might not be used but better be safe than sorry.
        },
        onAppReady() {
            this.buildNavbar();
            //Not yet using polling
            this.startPollers();
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
            this.forumStats = defaultState.forumStats;
            this.stopPollers();
            AxiosClient.ForceLogoff();
            this.previousRoute = undefined;
            router.replace({ name: 'login', query: { logoffReason: reason, logoffMethod: "manual" } });
        }
    }
});

router.beforeEach((to, from) => {
    document.title = `Forum - ${to.name as string}`;
    if(from && from.name) {
        useAppContextStore().previousRoute = {
            path: from.path,
            name: from.name as string,
            query: from.query,
            params: from.params
        }
    }
});