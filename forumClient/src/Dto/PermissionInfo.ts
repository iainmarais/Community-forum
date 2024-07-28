export type PermissionInfo = {
    id: number;
    name: string;
}

export type Permission = string;

export enum  PermissionType {
    Users_Create = "Users_Create",
    Users_Edit = "Users_Edit",
    Users_Delete = "Users_Delete",
    Users_ChangeRoles = "Users_ChangeRoles",
    Users_ChangePassword = "Users_ChangePassword",
    Users_BanUser = "Users_BanUser",
    Roles_Create = "Roles_Create",
    Roles_Edit = "Roles_Edit",
    Roles_Delete = "Roles_Delete",
    Threads_Create = "Threads_Create",
    Threads_Edit = "Threads_Edit",
    Threads_Delete = "Threads_Delete",
    Threads_Lock = "Threads_Lock",
    Threads_Unlock = "Threads_Unlock",
    Posts_Create = "Posts_Create",
    Posts_Edit = "Posts_Edit",
    Posts_Delete = "Posts_Delete",
    Posts_Update = "Posts_Update",
    Posts_PostImage = "Posts_PostImage",
    Posts_PostReply = "Posts_PostReply",
    Topics_Create = "Topics_Create",
    Topics_Edit = "Topics_Edit",
    Topics_Delete = "Topics_Delete",
    Chat_Create = "Chat_Create",
    Chat_Edit = "Chat_Edit",
    Chat_Delete = "Chat_Delete",
    Chat_PostImage = "Chat_PostImage",
    Chat_CreateGroup = "Chat_CreateGroup",
    Chat_EditGroup = "Chat_EditGroup",
    Chat_DeleteGroup = "Chat_DeleteGroup",
    Chat_JoinGroup = "Chat_JoinGroup",
    Gallery_UploadImage = "Gallery_UploadImage",
    Gallery_DeleteImage = "Gallery_DeleteImage",
    Gallery_EditImage = "Gallery_EditImage",
}

export type SystemPermissionEntry = {
    systemPermissionId: string,
    permission: PermissionType,
    description: string
}
