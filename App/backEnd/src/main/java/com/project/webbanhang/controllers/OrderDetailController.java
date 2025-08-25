package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.OrderDetailDTO;
import com.project.webbanhang.response.OrderDetailResponse;
import com.project.webbanhang.services.IOrderDetailService;

import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;

import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("${api.prefix}/order_details")
public class OrderDetailController {
	
	private final IOrderDetailService orderDetailService;

	// Done
    @PostMapping("")
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
            
            OrderDetailResponse existingOrderDetailResponse = orderDetailService.createOrderDetail(orderDetailDTO);
            
            return ResponseEntity.ok(existingOrderDetailResponse);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }

    // Done
    @GetMapping("/{id}")
    public ResponseEntity<?> getOrderDetail(@Valid @PathVariable("id") Long id) {
        try {
        	
        	 OrderDetailResponse existingOrderDetailResponse = orderDetailService.getOrderDetail(id);
        	
            return ResponseEntity.ok(existingOrderDetailResponse);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get order detail failed");
        }
    }

    // Done
    @GetMapping("/order/{order_id}")
    public ResponseEntity<?> getOrderDetailsByOrderId(@Valid @PathVariable("order_id") Long orderId) {
        try {
        	List<OrderDetailResponse> existingOrderDetailResponse = orderDetailService.getOrderDetailsByOrderId(orderId);
        	
            return ResponseEntity.ok(existingOrderDetailResponse);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }
    
    // Done
    @GetMapping("")
    public ResponseEntity<?> getOrderDetails() {
        try {
        	
        	List<OrderDetailResponse> existingOrderDetailResponses = orderDetailService.getOrderDetails();
        	
            return ResponseEntity.ok(existingOrderDetailResponses);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get order details failed");
        }
    }

    // Done
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
            
            OrderDetailResponse existingOrderDetailResponses = orderDetailService.updateOrderDetail(id, newOrderDetailDTO);
            
            return ResponseEntity.ok(existingOrderDetailResponses);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Update order detail failed");
        }
    }

    // Done
    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteOrderDetail(
            @Valid @PathVariable("id") Long id
    ) {
        try {
        	
        	orderDetailService.deleteOrderDetail(id);
        	
            return ResponseEntity.ok("Delete order detail with id: " + id);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Delete order detail failed");
        }
    }
}