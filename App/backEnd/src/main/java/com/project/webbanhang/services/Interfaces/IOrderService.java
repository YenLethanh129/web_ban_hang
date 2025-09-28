package com.project.webbanhang.services.Interfaces;

import java.util.List;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.response.OrderResponse;

public interface IOrderService {

	OrderResponse createOrder(String token, OrderDTO orderDTO) throws Exception;
	
	OrderResponse getOrderById(String extractedToken, Long orderId) throws Exception;
	
	List<OrderResponse> findByCustomer(String token) throws Exception;
	
	OrderResponse updateOrder(Long orderId, OrderDTO orderDTO) throws DataNotFoundException;
	
	List<OrderResponse> getAllOrders();
	
	void cancelOrder(String extractedToken, Long orderId) throws Exception;
}
