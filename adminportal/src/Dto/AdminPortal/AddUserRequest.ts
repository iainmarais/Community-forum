export type AddUserRequest = {
    //Most of these props can be generated in the backend code. What we do need from the admin frontend is the username, email address, password and their roleId, else default that to "User" if not provided by the frontend.
    username: string,
    password: string,
    emailAddress: string,
    roleId?: string,
}