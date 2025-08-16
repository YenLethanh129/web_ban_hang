package com.project.webbanhang.api;

import com.project.webbanhang.dtos.CreateMomoRequestDTO;
import com.project.webbanhang.response.CreateMomoResponse;
import org.springframework.cloud.openfeign.FeignClient;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

//@FeignClient(name = "momo", url = "${momo.endpoint}")
@FeignClient(name = "momo", url = "https://test-payment.momo.vn/v2/gateway/api")
public interface MomoApi {

    @PostMapping("/create")
    CreateMomoResponse createMomoQR(@RequestBody CreateMomoRequestDTO createMomoRequestDTO);
}
