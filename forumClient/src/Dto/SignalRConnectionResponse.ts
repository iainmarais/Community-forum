export type SignalRConnectionResponse =  {
    data: SignalRConnectionResponse;
    negotiateVersion: number;
    connectionId: string;
    connectionToken: string;
    availableTransports: AvailableTransport[];
}

export type AvailableTransport = {
    transport: string;
    transferFormats: string[];
}