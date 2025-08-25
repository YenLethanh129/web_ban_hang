export interface CreateMomoResponse {
    partnerCode: string;
    orderId: string;
    requestId: string;
    amount: number;
    responseTime: number;
    message: string;
    resultCode: number;
    payUrl: string;
    deeplink: string | null;
    qrCodeUrl: string | null;
}

export interface MomoIpnRequestDTO {
    orderType: string;
    amount: number;
    partnerCode: string;
    orderId: string;
    extraData: string;
    signature: string;
    transId: number;
    responseTime: number;
    resultCode: number;
    message: string;
    payType: string;
    requestId: string;
    orderInfo: string;
}