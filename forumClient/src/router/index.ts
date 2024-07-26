import { createRouter, createWebHistory, type RouteRecordRaw } from "vue-router";

export const LoginRoute = "login";
export const RegisterRoute = "register";
export const LogoffRoute = "logoff";
export const MainRoute = "home";
export const HomeRoute = "overview";
export const ChatRoute = "chat";
export const NotFoundRoute = "notfound";

const routes: RouteRecordRaw[] = [
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
                component: () => import("@/components/views/Forum/General/ChatView.vue")
            },
            {
                path: "/thread/:threadId",
                name: "ViewThread",
                component: () => import("@/components/views/Forum/Discussion/DiscussionView.vue"),
                props: true
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
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;
