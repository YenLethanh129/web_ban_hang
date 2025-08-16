package com.project.webbanhang.dtos;

import lombok.*;

/*
* MoMo gọi notifyUrl của backend để báo kết quả thanh toán
* */
@Getter
@Setter
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class CreateMomoRequestDTO {
    public String partnerCode;
    public String requestId;
    public long amount;
    public String orderId;
    public String orderInfo;
    public String redirectUrl;
    public String ipnUrl;
    public String requestType;
    public String extraData;
    public String lang;
    public String signature;
}
