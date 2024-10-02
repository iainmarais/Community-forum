import ConfigurationLoader from '@/config/ConfigurationLoader';
import type { ForumAppState } from '@/Dto/app/ForumAppState';
import ErrorHandler from '@/Handlers/ErrorHandler';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { mapToForumAppState } from './Mapper';

//Keep this at the module level.
var connection: HubConnection | null = null;
type ServerResponse = any;

export const createHubConnection = (hubName: string, logLevel: LogLevel = LogLevel.Information): HubConnection => {
    return new HubConnectionBuilder()
        .withUrl(`${ConfigurationLoader.getConfig().apiV1.baseUrl}/hubs/${hubName}`)
        .withAutomaticReconnect()
        .configureLogging(logLevel)
        .build();
};

export const initConnection = async (hubName: string, clientMethodName?: string): Promise<void> => {
    if(!connection || connection.state === HubConnectionState.Disconnected) {
        connection = createHubConnection(hubName);

        if(!clientMethodName) {
            //Client method name has not been provided.
            console.error("Client method name has not been provided or is invalid.");
        } 
        if (typeof clientMethodName !== "string") {
            console.error("Incorrect datatype provided for client method name.");
        }
        else {
            connection.on(clientMethodName, (data: any)=> {
                //Debug: log the response from the server.
                console.log(`A response from the server to ${clientMethodName} has been received`);
            });
        }

        try {
            await connection.start();
            console.log(`Connected to hub: ${hubName}`);

            //Log any connection state changes
            connection.onreconnecting(() => {
                console.warn(`Reconnecting to hub: ${hubName}`);
            });

            connection.onreconnected(() => {
                console.log(`Reconnected to hub: ${hubName}`);
            });

            connection.onclose((err) => {
                console.error(`Connection to hub: ${hubName} has been closed due to an error: ${err}.`);
                ErrorHandler.handleSignalRErrorResponse(`Connection to hub: ${hubName} has been closed due to an error: ${err}.`, err);
            });

        } catch (err) {
            console.error(`Error while starting connection to hub: ${hubName}`, err);
            ErrorHandler.handleSignalRErrorResponse(`Error while starting connection to hub: ${hubName}`, err);
        }
    }
};

export const sendSignalRMessage = async (hubMethodName: string, message: string, ): Promise<void> => {
    if(connection && connection.state === HubConnectionState.Connected) {
        try {
            console.log(`Message sent to ${hubMethodName}: ${message}`);
            //Wait for the server's response.
            const response: ServerResponse = await connection.invoke(hubMethodName, message);
            const forumAppState: ForumAppState = mapToForumAppState(response);
            //Debug: log the response from the server.
            console.log(`Response from server: ${response}`);
            console.log(forumAppState);
        }
        catch (err) {
            console.error(`Error while sending message to hub: ${hubMethodName}`, err);
            ErrorHandler.handleSignalRErrorResponse(`Error while sending message to hub: ${hubMethodName}`, err);
        }
    }

    else {
        console.warn("Connection not ready, or has been disconnected.");
    }

};

export const closeConnection = async (): Promise<void> => {
    if(connection) {
        try {
            await connection.stop();
            console.log("Connection closed.");
        }
        catch (err) {
            console.error("Error while closing connection.", err);
            ErrorHandler.handleSignalRErrorResponse("Error while closing connection.", err);
        }
    }
}