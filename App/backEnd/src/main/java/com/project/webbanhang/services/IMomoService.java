package com.project.webbanhang.services;

import com.project.webbanhang.dtos.MomoIpnRequestDTO;
import com.project.webbanhang.response.MomoCreateResponse;

public interface IMomoService {
    public MomoCreateResponse createPayment(long amount, String orderId, String orderInfo);
    public boolean verifyIpnSignature(MomoIpnRequestDTO ipn) throws Exception;

}
