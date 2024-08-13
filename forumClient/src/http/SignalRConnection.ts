import ErrorHandler from "@/Handlers/ErrorHandler";
import ConfigurationLoader from "@/config/ConfigurationLoader";
import * as signalR from "@microsoft/signalr";
import SignalRService from "@/services/SignalRService";

export type ConnectionRequest = {
    protocol: string,
    version: number,
    type: number,
    target: string,
    arguments: string[],
}
//Urls used by the server:
const signalRV1HttpBaseUrl = `${ConfigurationLoader.getConfig().apiV1.baseUrl}/hubs`;
const signalRV1WssBaseUrl = `${ConfigurationLoader.getConfig().signalRV1.baseUrl}/hubs`;

interface SignalRConnectionOptions {
    hubName: string;
    methodName: string;
    onReceiveMessage: (messageType: string, ...args: any[]) => void;
    userId?: string;
}


const negotiateConnection = async (hubName: string) => {
    const negotiationResponse = await SignalRService.postNegotiate(1, hubName);
    return negotiationResponse.data;
}

const createSignalRConnection = async ({hubName, methodName, onReceiveMessage, userId} : SignalRConnectionOptions): Promise<signalR.HubConnection> => {

    const connection = new signalR.HubConnectionBuilder()
    .withUrl(`${signalRV1WssBaseUrl}/${hubName}?userId=${userId}`,{
        transport: signalR.HttpTransportType.WebSockets,
        skipNegotiation: true,
    })
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

    //Connection handlers for receiving messages:
    connection.on(methodName, async (messageType: string, ...args: any[]) => {
       await onReceiveMessage(messageType, ...args);
    });

    connection.onclose(error => {
        if(error) {
            ErrorHandler.handleApiErrorResponse(error);
        }
        console.warn(`Connection to hub ${hubName} closed.`);
    });

    try {
        await connection.start();
        console.log(`Connection to hub ${hubName} established`);
    }
    catch (error) {
        ErrorHandler.handleApiErrorResponse(error);
        throw new Error(`Connection to hub ${hubName} failed. Please try again later.`);
    }

    return connection;
}

export default {
    createSignalRConnection,
    negotiateConnection
};