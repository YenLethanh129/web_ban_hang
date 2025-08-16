package com.project.webbanhang.controllers;

import com.project.webbanhang.response.CreateMomoResponse;
import com.project.webbanhang.services.MomoService;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/momo")
@RequiredArgsConstructor
public class MomoController {
    private final MomoService momoService;

    @PostMapping("/create")
    public CreateMomoResponse createQR(){
        return momoService.createQR();
    }
}
