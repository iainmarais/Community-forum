import { defineStore } from "pinia";
import SignalRConnection from "@/http/SignalRConnection";
import * as signalR from "@microsoft/signalr";
import ErrorHandler from "@/Handlers/ErrorHandler";

type SignalRState = {
    signalRConnection: signalR.HubConnection | null,
    isConnected: boolean,
    connectionInProgress: boolean,
    messages: string[],
}

const defaultState: SignalRState = {
    signalRConnection: null,
    isConnected: false,
    connectionInProgress: false,
    messages: [],
}

export const useSignalRConnectionStore = defineStore({
    id: "SignalRConnectionStore",
    state: () => (defaultState),
    getters: {},
    actions: {
        async negotiateConnection(userId:string) {
            //For now, execute the postNegotiate action on the helper library and log the outputs. The idea is to determine which method to use, preferably websockets.
            var response = await SignalRConnection.negotiateConnection(userId);
            if(response.data) {
                console.log(`Available transports: ${response.data.availableTransports}`);
            }
        },
        async connectToHub(hubName: string, methodName: string, onReceiveMessage: (messageType: string, ...args: any[]) => void, userId?: string) {
            try {
                const options = {
                    hubName: hubName,
                    methodName: methodName,
                    onReceiveMessage: onReceiveMessage,
                    userId: userId
                };
                this.signalRConnection = await SignalRConnection.createSignalRConnection(options);
                this.isConnected = true;
            } catch (error) {
                ErrorHandler.handleApiErrorResponse(error);
                console.error(error)
            }
        }, 
        async disconnectFromHub() {
            if(this.signalRConnection) {
                this.signalRConnection.stop();
                this.signalRConnection = null;
                this.isConnected = false;
            }
        },
        onReceiveMessage(messageType: string, ...args: any[]) {
            this.messages.push(`Type: ${messageType}, Args: ${JSON.stringify(args)}`);
        },
        async sendMessage(methodName: string, ...args: any[]) {
            if(this.signalRConnection) {
                try {
                    //The args I need to push include the protocol, version, type and target, and the userId.
                    await this.signalRConnection.invoke(methodName, JSON.stringify(args));
                } catch (error) {
                    console.log(error);
                    ErrorHandler.handleApiErrorResponse(error);
                }
            }
        }
    }
});