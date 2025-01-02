package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.OrderDetailDTO;
import com.project.webbanhang.models.OrderDetail;
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
            
            OrderDetail existingOrderDetail = orderDetailService.createOrderDetail(orderDetailDTO);
            
            return ResponseEntity.ok(existingOrderDetail);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }

    // Done
    @GetMapping("/{id}")
    public ResponseEntity<?> getOrderDetail(@Valid @PathVariable("id") Long id) {
        try {
        	
        	OrderDetail existingOrderDetail = orderDetailService.getOrderDetail(id);
        	
            return ResponseEntity.ok(existingOrderDetail);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get order detail failed");
        }
    }

    // Done
    @GetMapping("/order/{order_id}")
    public ResponseEntity<?> getOrderDetailByOrderId(@Valid @PathVariable("order_id") Long orderId) {
        try {
        	
        	OrderDetail existingOrderDetail = orderDetailService.getOrderDetailByOrderId(orderId);
        	
            return ResponseEntity.ok(existingOrderDetail);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get order details failed");
        }
    }
    
    // Done
    @GetMapping("")
    public ResponseEntity<?> getOrderDetails() {
        try {
        	
        	List<OrderDetail> existingOrderDetails = orderDetailService.getOrderDetails();
        	
            return ResponseEntity.ok(existingOrderDetails);
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
            
            OrderDetail orderDetail = orderDetailService.updateOrderDetail(id, newOrderDetailDTO);
            
            return ResponseEntity.ok(orderDetail);
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