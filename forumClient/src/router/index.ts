import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router";
import { Last_Route } from "@/LocalStorage/keys";
import NProgress from "NProgress";

export const LoginRoute = "login";
export const RegisterRoute = "register";
export const LogoffRoute = "logoff";
export const MainRoute = "";
export const HomeRoute = "home";
export const ChatRoute = "chat";
export const GalleryRoute = "gallery";
export const SearchRoute = "search";
export const NotFoundRoute = "notfound";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: "/",
            name: MainRoute,
            component: () => import("@/components/pages/MainPage.vue"),
            children:[
                {
                    path: "/logoff",
                    name: LogoffRoute,
                    component: () => import("@/components/views/Forum/General/LogoffView.vue")
                },
                {
                    //Add subroutes for the overview, discussion view, thread list view, chat history list view etc?
                    path: "/home",
                    name: HomeRoute,
                    component: () => import("@/components/views/Forum/General/HomeView.vue")
                },
                {
                    path: "/register",
                    name: RegisterRoute,
                    component: () => import("@/components/pages/RegistrationPage.vue")
                },
                {
                    path: "/chat",
                    name: ChatRoute,
                    component: () => import("@/components/views/Forum/Chat/ChatView.vue")
                },
                {
                    path: "/gallery",
                    name: GalleryRoute,
                    component: () => import("@/components/views/Forum/Gallery/GalleryView.vue")
                },
                {
                    path: "/search",
                    name: SearchRoute,
                    component: () => import("@/components/views/Forum/Search/SearchView.vue")
                },
                {
                    path: "/thread/:threadId",
                    name: "ViewThread",
                    component: () => import("@/components/views/Forum/Discussion/DiscussionView.vue"),
                },
                {
                    path: "topic/:topicId",
                    name: "ViewTopic",
                    component: () => import("@/components/views/Forum/Topics/TopicView.vue"),
                },
                {
                    path: "category/:categoryId",
                    name: "ViewCategory",
                    component: () => import("@/components/views/Forum/Categories/CategoryView.vue"),
                },
                {
                    path: "board/:boardId",
                    name: "ViewBoard",
                    component: () => import("@/components/views/Forum/Boards/BoardView.vue"),
                },
            ]
        },
        {
            path: "/:catchAll(.*)",
            name: NotFoundRoute,
            component: () => import("@/components/pages/LocationNotFoundPage.vue"),
        },
        {
            path: "/login",
            name: LoginRoute,
            component: () => import("@/components/pages/LoginPage.vue")
        },
    ]
});


router.beforeEach(async (to, from) =>{
    NProgress.start();
    if(to.name != LoginRoute) {
        const storeRoute = {
            name: to.name,
            params: to.params,
            query: to.query
        };
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
