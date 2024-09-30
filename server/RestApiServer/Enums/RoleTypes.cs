namespace RestApiServer.Enums
{
    public enum RoleType
    {
        Admin,  // Full access to manage the forum
        Moderator,      // Monitors activities and enforces rules
        Creator, // Produces high-quality content
        Manager, // Engages the community and organizes events
        Expert, // Recognized for expertise in specific areas
        User,    // Standard members with posting privileges
        Guest,          // Non-registered users with limited access
        Banned,     // Users banned from the forum
        Support,   // Assists users with forum-related issues
        JuniorModerator, // Moderators-in-training with limited permissions
        Sponsor         // Users/entities sponsoring the forum
    }
    //sql to create the Roles table, roleId is the primary key, roletype is the same value as the enum, rolename is a friendly display name, and description is the same as for the comment next to each role type.
    /*
    INSERT INTO roles (RoleId, RoleType, RoleName, Description)
    VALUES
        (1, 'Administrator', 'admin', 'Full access to manage the forum'),
        (2, 'Moderator', 'moderator', 'Monitors activities and enforces rules'),
        (3, 'Content Creator', 'creator', 'Produces high-quality content'),
        (4, 'Community Manager', 'manager', 'Engages the community and organizes events'),
        (5, 'Expert', 'expert', 'Recognized for expertise in specific areas'),
        (6, 'User', 'user', 'Standard members with posting privileges'),
        (7, 'Guest', 'guest', 'Non-registered users with limited access'),
        (8, 'Banned', 'banned', 'Users banned from the forum'),
        (9, 'Support', 'support', 'Assists users with forum-related issues'),
        (10, 'Junior Moderator', 'junior_moderator', 'Moderators-in-training with limited permissions'),
        (11, 'Sponsor', 'sponsor', 'Users/entities sponsoring the forum');


    */ 
}