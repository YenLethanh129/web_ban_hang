package com.project.webbanhang.dtos;

import lombok.Data;

@Data
/*
* MoMo gọi notifyUrl của backend để báo kết quả thanh toán
* */
public class MomoIpnRequestDTO {
    public String partnerCode;
    public String orderId;
    public String requestId;
    public long amount;
    public long transId;
    public String orderInfo;
    public String orderType;
    public int resultCode;
    public String message;
    public String payType;
    public long responseTime;
    public String extraData;
    public String signature;
}
