import type { ForumAppState } from "@/Dto/app/ForumAppState";
//Add more as needed.

export const mapToForumAppState = (data: any): ForumAppState => {
    return {
        forumStats: data.forumStats,
        loggedInUser: data.loggedInUser
    }
}