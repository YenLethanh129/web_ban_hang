package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.momo.MomoInfoOrderDTO;
import com.project.webbanhang.dtos.momo.MomoIpnRequestDTO;
import com.project.webbanhang.response.CreateMomoResponse;
import com.project.webbanhang.response.OrderResponse;
import com.project.webbanhang.services.IMomoService;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/momo")
@RequiredArgsConstructor
public class MomoController {
    private final IMomoService momoService;

    @PostMapping("/create")
    public CreateMomoResponse createQR(
            @RequestBody MomoInfoOrderDTO momoInfoOrderDTO
    ){
        return momoService.createQR(momoInfoOrderDTO);
    }

    /**
     * Momo return answer (Service to service)
     */
    @PostMapping("/ipnHandler")
    public OrderResponse inpHandler(
            @RequestBody MomoIpnRequestDTO momoIpnRequestDTO
    ){
        return momoService.ipnHandler(momoIpnRequestDTO);
    }
}
