package com.project.webbanhang.services;

import java.util.Date;
import java.util.List;

import org.modelmapper.ModelMapper;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.Order;
import com.project.webbanhang.models.User;
import com.project.webbanhang.repositories.OrderRepository;
import com.project.webbanhang.repositories.UserRepository;
import com.project.webbanhang.response.OrderResponse;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class OrderService implements IOrderService{
	private final OrderRepository orderRepository;
	private final UserRepository userRepository;
	private final ModelMapper modelMapper;
	
	@Override
	public OrderResponse createOrder(OrderDTO orderDTO) throws DataNotFoundException {
		// tim user
		User user = userRepository.findById(orderDTO.getUserId())
				.orElseThrow(() -> new DataNotFoundException("Can't not found user with id " + orderDTO.getUserId()));
		
		// Dùng thư viện Model Maper để chuyển đổi
		modelMapper.typeMap(OrderDTO.class, Order.class)
			.addMappings(mapper -> mapper.skip(Order::setId));
		Order order = new Order();
		modelMapper.map(orderDTO, order);
		order.setUser(user);
		order.setOrderDate(new Date());
		order.setStatus("PENDING");
		order.setIsActive(true);
		orderRepository.save(order);
		
        modelMapper.typeMap(Order.class, OrderResponse.class)
			.addMappings(mapper -> mapper.skip(OrderResponse::setUserId));
        OrderResponse existingOrderResponse = new OrderResponse();
        modelMapper.map(order, existingOrderResponse);
        existingOrderResponse.setUserId(user.getId());
		return existingOrderResponse;
	}

	@Override
	public Order getOrderById(Long orderId) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Order updateOrder(Long orderId) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public List<Order> getAllOrders(Long userId) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public void deleteOrder(Long orderId) {
		// TODO Auto-generated method stub
		
	}

}
