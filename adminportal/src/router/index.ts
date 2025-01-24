import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router";
import { Last_Route } from "@/LocalStorage/keys";
import nprogress from "nprogress";


export const NotFoundRoute = "notfound";
export const HomeRoute = "home";
export const LoginRoute = "login";
export const LogoffRoute = "logoff";
export const ContentManagementRoute = "contentmgmt";
export const UserManagementRoute = "usermgmt";
export const PermissionsManagement = "permissionsmgmt";
export const SupportRequestsRoute = "supportrequests";
export const MainRoute = "";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: "/",
            name: MainRoute,
            component: () => import("@/pages/MainPage.vue"),
            children: [
                {
                    path: "/home",
                    name: HomeRoute,
                    //Todo: build out.
                    component: () => import("@/components/Views/Overview.vue")
                },
                {
                    path: "/contentmgmt",
                    name: ContentManagementRoute,
                    component: () => import("@/components/Views/ContentManagementView.vue")
                },
                {
                    path: "/logoff",
                    name: LogoffRoute,
                    component: () => import("@/components/Views/LogoffView.vue")
                },
                {
                    path: "/usermgmt",
                    name: UserManagementRoute,
                    component: () => import("@/components/Views/UserManagementView.vue")
                },
                {
                    path: "/supportrequests",
                    name: SupportRequestsRoute,
                    component: () => import("@/components/Views/ServiceRequestsView.vue")
                },
                {
                    path: "/permissionsmgmt",
                    name: PermissionsManagement,
                    component: () => import("@/pages/PageUnderconstruction.vue")
                }
            ]
        },
        {
            path: "/login",
            name: LoginRoute,
            component: () => import("@/pages/LoginPage.vue")
        },
        //Catch all
        {
            path: "/:catchAll(.*)",
            name: NotFoundRoute,
            component: () => import("@/pages/LocationNotFoundPage.vue"),
        },
    ]
});

router.beforeEach(async (to, from) =>{
    nprogress.start();
    //Todo: correct this once the name has been created.
    if(to.name !== LoginRoute) {
        const storeRoute = {
            name: to.name,
            params: to.params,
            query: to.query
        }
        console.log("Route stored: "+ JSON.stringify(storeRoute));
        localStorage.setItem(Last_Route, JSON.stringify(storeRoute));
    }

});

router.afterEach(async (to, from) => {
    nprogress.done();
});

router.onError((error) => {
    console.error(error);
    if(error.message.includes("Failed to fetch dynamically imported module") || error.message.includes("Importing a module script failed")) {
        setTimeout(() => {
            location.reload();
        }, 5000);
    }
});

export default router;