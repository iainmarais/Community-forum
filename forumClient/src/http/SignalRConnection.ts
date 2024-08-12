import type { UserBasicInfo } from "@/Dto/UserInfo";
import ErrorHandler from "@/Handlers/ErrorHandler";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import UserService from "@/services/UserService";
import * as signalR from "@microsoft/signalr";

//Get the user's info from the server using an API call.
const getUserInfo = async (userId: string): Promise<UserBasicInfo> => {
    try {
        const response = await UserService.getUserById(userId);
        return response.data;
    } catch (err) {
        ErrorHandler.handleApiErrorResponse(err);
        return {} as UserBasicInfo; // Return an empty object if there's an error
    }
};

type connectionParam = {
    name: string,
    value: string,
}

//The idea is to be able to instance the connection with different parameters, eg for chat and forum stats.
//This also means the hub name will be different. 
//Currently only chat is available.

export const createSignalRConnection = (
    hubName: string,
    connectionParams: connectionParam[]
) => {
    const paramString = connectionParams
        .map((param) => `${encodeURIComponent(param.name)}=${encodeURIComponent(param.value)}`)
        .join("&");

    const url = `${ConfigurationLoader.getConfig().apiV1.baseUrl}/hubs/${hubName}?${paramString}`;

    const conn = new signalR.HubConnectionBuilder()
        .withUrl(url)
        .build();

    conn.on("ReceiveMessage", async (userId:string, message:string) => {
        try {
            const user = await getUserInfo(userId);
            console.log(user);
            console.log(message);
        } catch (err) {
           ErrorHandler.handleApiErrorResponse(err); 
        }
    });

    conn.start()
        .then(() => console.log(`Connection to hub ${hubName} started`))
        .catch((err) => ErrorHandler.handleApiErrorResponse(err));

    return conn;
};