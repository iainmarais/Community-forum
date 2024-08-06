export type ContactEntry = {
    contactId: string;
    contactName: string;
    contactEmailAddress: string;
    aboutMessage: string;
    contactProfileImageBase64: string;
    createdDate: string;
}

export type ContactBasicInfo = {
    contact: ContactEntry
}