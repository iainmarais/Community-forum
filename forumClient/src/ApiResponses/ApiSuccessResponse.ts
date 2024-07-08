import type { AllowedUserActions } from '@/Actions/AllowedUserActions';

export interface ApiSuccessResponse<T, M = any | undefined> {
    message: string,
    data: T,
    metaData: M,
    actions?: AllowedUserActions
}