package com.project.webbanhang.services;

import java.util.List;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.Order;
import com.project.webbanhang.response.OrderResponse;

public interface IOrderService {

	OrderResponse createOrder(OrderDTO orderDTO) throws DataNotFoundException;
	
	Order getOrderById(Long orderId);
	
	Order updateOrder(Long orderId);
	
	List<Order> getAllOrders(Long userId);
	
	void deleteOrder(Long orderId);
}
