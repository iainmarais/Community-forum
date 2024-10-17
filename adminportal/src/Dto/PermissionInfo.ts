export type PermissionEntry = {
    permissionId: string,
    permissionName: string,
    permissionType: PermissionType,
    description: string
}

export type SystemPermissionEntry = {
    systemPermissionId: string,
    systemPermissionType: SystemPermissionType,
    systemPermissionName: string,
    description: string,
}

export enum  SystemPermissionType {
    Visibility,
    Access,
    Interactivity,
    General,
    Development,
    Testing,
    Production
    //Add more as needed.
}

export enum PermissionType {
    Administrative,
    Content,
    General,
    Moderation
    //Add more as needed.
}