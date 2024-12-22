package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.response.OrderResponse;
import com.project.webbanhang.services.IOrderService;

import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;

import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("${api.prefix}/orders")
public class OrderController {
	
	private final IOrderService orderService;

	// Done
    @PostMapping("")
    public ResponseEntity<?> createOrder(
            @RequestBody @Valid OrderDTO orderDTO,
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
            
            OrderResponse existingOrderResponse = orderService.createOrder(orderDTO);
            
            return ResponseEntity.ok(existingOrderResponse);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Create order failed");
        }
    }
    
    @GetMapping("/{order_id}")
    public ResponseEntity<?> getOrder(
    		@Valid @PathVariable("order_id") Long userId
    ) {
        try {
        	
        	orderService.findByUserId(userId);
        	
            return ResponseEntity.ok("Get orders by user id: " + userId);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get orders failed");
        }
    }
    
    @GetMapping("")
    public ResponseEntity<?> getAllOrders(
    ) {
        try {

            return ResponseEntity.ok("Get orders");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get orders failed");
        }
    }

    @GetMapping("/user/{user_id}")
    public ResponseEntity<?> getOrdersByUserId(
    		@Valid @PathVariable("user_id") Long userId
    ) {
        try {
        	
        	List<OrderResponse> existingOrderResponses = orderService.findByUserId(userId);
        	
            return ResponseEntity.ok(existingOrderResponses);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get orders failed");
        }
    }

    @PutMapping("/{id}")
    public ResponseEntity<?> updateOrder(
            @PathVariable("id") Long id,
            @RequestBody @Valid OrderDTO orderDTO,
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
            return ResponseEntity.ok("Update order with id: " + id + ", orderDTO: " + orderDTO.getNote());
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Update order failed");
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteOrder(@PathVariable("id") Long id) {
        try {
            return ResponseEntity.ok("Delete order with id: " + id);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Delete order failed");
        }
    }
}
