package com.project.webbanhang.services;

import com.project.webbanhang.dtos.momo.MomoInfoOrderDTO;
import com.project.webbanhang.response.CreateMomoResponse;

public interface IMomoService {
    CreateMomoResponse createQR(MomoInfoOrderDTO momoInfoOrderDTO);
}
