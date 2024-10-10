import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router";
import { Last_Route } from "@/LocalStorage/keys";
import NProgress from "NProgress";


export const NotFoundRoute = "notfound";
export const HomeRoute = "home";
export const LoginRoute = "login";
export const ContentManagementRoute = "contentmgmt";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: "/",
            name: "Home",
            component: () => import("@/pages/MainPage.vue"),
            children: []
        },
        {
            path: "/login",
            name: LoginRoute,
            component: () => import("@/pages/LoginPage.vue")
        },
        {
            path: "/contentmgmt",
            name: ContentManagementRoute,
            component: () => import("@/pages/PageUnderconstruction.vue")
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
    NProgress.start();
    //Todo: correct this once the name has been created.
    if(to.name !== "Name_of_Login_Route") {
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
    NProgress.done();
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