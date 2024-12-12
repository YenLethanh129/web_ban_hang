package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.OrderDTO;
import jakarta.validation.Valid;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("${api.prefix}/orders")
public class OrderController {

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
            return ResponseEntity.ok("Create order successfully orderDTO: " + orderDTO.getNote());
        } catch (Exception e) {
            return ResponseEntity.badRequest().body("Create order failed");
        }
    }

    @GetMapping("/{user_id}")
    public ResponseEntity<?> getOrdersByUserId(@Valid @PathVariable("user_id") Long userId) {
        try {
            return ResponseEntity.ok("Get orders by user id: " + userId);
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
