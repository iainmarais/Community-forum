export type PermissionInfo = {
    id: number;
    name: string;
}

export type Permission = string;

export enum  PermissionType {
    //Todo: build out.
}
//Whether this is used or not depends on the design and the user context. For user context, it can't be "forum", but "admin".
export type SystemPermissionEntry = {
    systemPermissionId: string,
    permission: PermissionType,
    description: string
}
