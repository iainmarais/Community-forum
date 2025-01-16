namespace RestApiServer.CommonEnums
{
    public enum RoleType
    {
        Admin,  // Full access to manage the forum
        CommunityManager, // Trusted members of the community with special privileges
        Moderator,      // Monitors activities and enforces rules
        ContentCreator, // Produces high-quality content
        Manager, // Engages the community and organizes events
        Expert, // Recognized for expertise in specific areas
        User,    // Standard members with posting privileges
        Guest,          // Non-registered users with limited access
        Banned,     // Users banned from the forum
        Support,   // Assists users with forum-related issues
        JuniorModerator, // Moderators-in-training with limited permissions
        Sponsor         // Users/entities sponsoring the forum
    }
}