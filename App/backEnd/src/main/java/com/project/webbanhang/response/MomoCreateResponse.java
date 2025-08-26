package com.project.webbanhang.response;

import lombok.Data;

@Data
/*
* Chứa thông tin MoMo trả về khi tạo đơn (URL thanh toán, QR code…).
* */
public class MomoCreateResponse {
    private String payUrl;
    private String deeplink;
    private String qrCodeUrl;
    private String requestId;
    private String orderId;
    private Integer resultCode;
    private String message;
}
