package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.OrderDetailDTO;
import jakarta.validation.Valid;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("${api.prefix}/order_details")
public class OrderDetailController {

    @PostMapping
    public ResponseEntity<?> createOrderDetail(
            @Valid @RequestBody OrderDetailDTO orderDetailDTO,
            BindingResult result
    ) {
        try {
            if (result.hasErrors()) {
                List<String> errorMessages = result.getFieldErrors()
                        .stream()
                        .map(FieldError::getDefaultMessage)
                        .toList();
                return ResponseEntity.badRequest().body(errorMessages);
            }
            return ResponseEntity.ok("Create order detail successfully orderDetailDTO: " + orderDetailDTO);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Create order detail failed");
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity<?> getOrderDetail(@Valid @PathVariable("id") Long id) {
        try {
            return ResponseEntity.ok("Get order detail by id: " + id);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get order detail failed");
        }
    }

    @GetMapping("/order/{order_id}")
    public ResponseEntity<?> getOrderDetails(@Valid @PathVariable("order_id") Long orderId) {
        try {
            return ResponseEntity.ok("Get order details by order id: " + orderId);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get order details failed");
        }
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateOrderDetail(
            @PathVariable("id") Long id,
            @Valid @RequestBody OrderDetailDTO newOrderDetailDTO,
            BindingResult result
    ) {
        try {
            if (result.hasErrors()) {
                List<String> errorMessages = result.getFieldErrors()
                        .stream()
                        .map(FieldError::getDefaultMessage)
                        .toList();
                return ResponseEntity.badRequest().body(errorMessages);
            }
            return ResponseEntity.ok("Update order detail with id: " + id + ", orderDetailDTO: " + newOrderDetailDTO);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Update order detail failed");
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteOrderDetail(
            @Valid @PathVariable("id") Long id
    ) {
        try {
            return ResponseEntity.ok("Delete order detail with id: " + id);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Delete order detail failed");
        }
    }
}