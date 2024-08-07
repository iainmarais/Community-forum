export type ContactEntry = {
    contactId: string;
    userId: string;
    createdByUserId: string;
    contactName: string;
    contactEmailAddress: string;
    aboutMessage: string;
    contactProfileImageBase64: string;
    createdDate: string;
}

export type ContactBasicInfo = {
    contact: ContactEntry
}