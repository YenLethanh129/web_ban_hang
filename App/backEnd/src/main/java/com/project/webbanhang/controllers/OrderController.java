package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.models.User;
import com.project.webbanhang.response.OrderResponse;
import com.project.webbanhang.services.Interfaces.IOrderService;

import com.project.webbanhang.utils.CookieToken;
import jakarta.servlet.http.Cookie;
import jakarta.servlet.http.HttpServletRequest;
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

    @GetMapping("/{order_id}")
    public ResponseEntity<?> getOrderById(
            @PathVariable("order_id") Long orderId,
            HttpServletRequest request
    ) {
        try {
            String extractedToken = CookieToken.extractTokenFromCookies(request);
            OrderResponse existingOrderResponse = orderService.getOrderById(extractedToken, orderId);

            return ResponseEntity.ok(existingOrderResponse);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }

	// Done
    @PostMapping("")
    public ResponseEntity<?> createOrder(
            @RequestBody @Valid OrderDTO orderDTO,
            HttpServletRequest request,
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
            Cookie[] cookies = request.getCookies();
            String extractedToken = null;
            if (cookies != null) {
                for (Cookie cookie : cookies) {
                    if (cookie.getName().equals("JWT_TOKEN")) {
                        extractedToken = cookie.getValue();
                        break;
                    }
                }
            }
            
            OrderResponse existingOrderResponse = orderService.createOrder(extractedToken, orderDTO);
            
            return ResponseEntity.ok(existingOrderResponse);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }

    // Done
    /**
     * TOP 10 OWASP 2023
     * API1:2023 - Broken Object Level Authorization (BOLA)
     * Hacker có thể lấy id của người dùng khác và truy cập vào thông tin cá nhân của họ
     * Giải pháp: Sử dụng token để xác thực người dùng hiện tại và chỉ trả về thông tin của họ
     * */
    @PostMapping("/user")
    public ResponseEntity<?> getOrdersByUserId(
            HttpServletRequest request
    ) {
        try {
            Cookie[] cookies = request.getCookies();
            String extractedToken = null;
            if (cookies != null) {
                for (Cookie cookie : cookies) {
                    if (cookie.getName().equals("JWT_TOKEN")) {
                        extractedToken = cookie.getValue();
                        break;
                    }
                }
            }
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
