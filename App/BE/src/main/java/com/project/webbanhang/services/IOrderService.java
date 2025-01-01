package com.project.webbanhang.services;

import java.util.List;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.response.OrderResponse;

public interface IOrderService {

	OrderResponse createOrder(OrderDTO orderDTO) throws DataNotFoundException;
	
	OrderResponse getOrderById(Long orderId) throws DataNotFoundException;
	
	List<OrderResponse> findByUserId(Long userId);
	
	OrderResponse updateOrder(Long orderId, OrderDTO orderDTO) throws DataNotFoundException;
	
	List<OrderResponse> getAllOrders();
	
	void deleteOrder(Long orderId);
}
