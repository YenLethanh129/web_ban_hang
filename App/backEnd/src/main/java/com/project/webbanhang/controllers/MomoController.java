package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.CreatePaymentInputDTO;
import com.project.webbanhang.dtos.MomoIpnRequestDTO;
import com.project.webbanhang.response.MomoCreateResponse;
import com.project.webbanhang.services.MomoService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.HashMap;
import java.util.Map;

import static javax.xml.catalog.BaseEntry.CatalogEntryType.URI;

@RestController
@RequestMapping("/payment/momo")
@RequiredArgsConstructor
public class MomoController {
    private final MomoService momoService;

    @PostMapping("/create")
    public ResponseEntity<?> create(@Validated @RequestBody CreatePaymentInputDTO input) {
        String orderId = (input.getOrderId() != null) ? input.getOrderId() : String.valueOf(System.currentTimeMillis());
        String orderInfo = (input.getOrderInfo() != null) ? input.getOrderInfo() : "Thanh toan don hang " + orderId;

        MomoCreateResponse res = momoService.createPayment(input.getAmount(), orderId, orderInfo);
        return ResponseEntity.ok(res);
    }

    @GetMapping("/return")
    public ResponseEntity<?> returnPage(@RequestParam Map<String, String> params) {
        return ResponseEntity.status(302).location(URI.create("/")).build();
    }

    @PostMapping("/notify")
    public ResponseEntity<?> notify(@RequestBody MomoIpnRequestDTO ipn) {
        boolean ok = momoService.verifyIpnSignature(ipn);
        Map<String, Object> resp = new HashMap<>();

        if (!ok) {
            resp.put("resultCode", 5);
            resp.put("message", "signature invalid");
            return ResponseEntity.ok(resp);
        }

        // TODO: cập nhật trạng thái đơn hàng

        resp.put("resultCode", 0);
        resp.put("message", "confirm success");
        return ResponseEntity.ok(resp);
    }
}
