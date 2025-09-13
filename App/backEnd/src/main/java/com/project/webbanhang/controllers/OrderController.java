package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.models.User;
import com.project.webbanhang.response.OrderResponse;
import com.project.webbanhang.services.Interfaces.IOrderService;

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
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }
    
    // Done
//    @GetMapping("")
//    public ResponseEntity<?> getAllOrders(
//    ) {
//        try {
//        	List<OrderResponse> existingOrderResponses = orderService.getAllOrders();
//
//            return ResponseEntity.ok(existingOrderResponses);
//        } catch (Exception e) {
//            return ResponseEntity.badRequest().body("Get orders failed");
//        }
//    }

    // Done
    @PostMapping("/user")
    public ResponseEntity<?> getOrdersByUserId(
            @RequestHeader("Authorization") String token
    ) {
        try {
            String extractedToken = token.substring(7);
        	List<OrderResponse> existingOrderResponses = orderService.findByCustomer(extractedToken);

            return ResponseEntity.ok(existingOrderResponses);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Get orders failed");
        }
    }

    // Done
    @PutMapping("/{id}")
    public ResponseEntity<?> updateOrder(
            @PathVariable("id") Long orderId,
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
            
            OrderResponse orderResponse = orderService.updateOrder(orderId, orderDTO);
            
            return ResponseEntity.ok(orderResponse);
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
