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
    Messages_Create = "Messages_Create",
    Messages_Edit = "Messages_Edit",
    Messages_Delete = "Messages_Delete",
    Messages_Update = "Messages_Update",
    Messages_PostImage = "Messages_PostImage",
    Messages_PostReply = "Messages_PostReply",
    Topics_Create = "Topics_Create",
    Topics_Edit = "Topics_Edit",
    Topics_Delete = "Topics_Delete"
}

export type SystemPermissionEntry = {
    systemPermissionId: string,
    permission: PermissionType,
    description: string
}
