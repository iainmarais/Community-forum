export type UpdateRequestTriageStatusRequest = {
    triageType: TriageType;
    triageStatus: TriageStatus;
}


export enum TriageType {
    Untriaged, //Default status - Untriaged: Not yet assessed.
    Low, //Low priority issues - will be tended to as needed. Typically feature requests.
    Normal, //Normal priority issues - will be tended to as part of standard operating procedures.
    High, //High priority issues - issues marked high priority need to be resolved urgently but can wait if warranted.
    Urgent //Urgent issues - issues marked urgent need to be resolved as a matter of utmost urgency. Everything else must wait.
}
export enum TriageStatus {
    Unspecified, //This request has not been classified.
    FeatureRequest, //This is a feature request, typically for an improvement or alteration to how the frontend works
    Blocker, //Blockers are issues that need to be addressed in order to fix other issues or address feature requests.
    Bug, //Every other type of fault that occurs with either the frontend or backend.
}