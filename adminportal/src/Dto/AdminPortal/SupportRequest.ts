export type SupportRequest = {
    //RequestId is auto-genned by the db.
    CreatedDate: number,
    Username: string,
    EmailAddress: string,
    Subject: string,
    Message: string
}