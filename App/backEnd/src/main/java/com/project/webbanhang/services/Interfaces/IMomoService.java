package com.project.webbanhang.services.Interfaces;

import com.project.webbanhang.dtos.momo.MomoInfoOrderDTO;
import com.project.webbanhang.dtos.momo.MomoIpnRequestDTO;
import com.project.webbanhang.response.CreateMomoResponse;
import com.project.webbanhang.response.OrderResponse;

public interface IMomoService {
    CreateMomoResponse createQR(MomoInfoOrderDTO momoInfoOrderDTO);

    OrderResponse ipnHandler(MomoIpnRequestDTO momoIpnRequestDTO);
}
