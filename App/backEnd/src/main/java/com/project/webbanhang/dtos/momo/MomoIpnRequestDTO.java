package com.project.webbanhang.dtos.momo;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class MomoIpnRequestDTO {
    private String orderType;
    private Long amount;
    private String partnerCode;
    private String orderId;
    private String extraData;
    private String signature;
    private Long transId;
    private Long responseTime;
    private Integer resultCode;
    private String message;
    private String payType;
    private String requestId;
    private String orderInfo;
}
