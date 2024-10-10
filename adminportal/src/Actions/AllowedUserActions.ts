export type AllowedUserActions = {
    [entityId: string]: AllowedUserAction[]
}

export type AllowedUserAction = {
    actionId: AllowedUserActionId
}

export type AllowedUserActionId = 
    'viewPost' |
    'reply' | 
    'editPost' | 
    'deletePost' | 
    'lockPost' | 
    'unlockPost' | 
    'hidePost' | 
    'unhidePost' | 
    'pinPost' | 
    'unpinPost' | 
    'hideUser' | 
    'unhideUser'