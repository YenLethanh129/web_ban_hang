package com.project.webbanhang.services;

import java.util.ArrayList;
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
		
		return mapOrderToOrderResponse(order);
	}

	@Override
	public OrderResponse getOrderById(Long orderId) throws DataNotFoundException {
		
		Order existingOrder = orderRepository.findById(orderId)
				.orElseThrow(() -> new DataNotFoundException("Can't found order with id: " + orderId));
		
		return mapOrderToOrderResponse(existingOrder);
	}

	@Override
	public OrderResponse updateOrder(Long orderId, OrderDTO orderDTO) throws DataNotFoundException {
		Order existingOrder = orderRepository.findById(orderId)
				.orElseThrow(() -> new DataNotFoundException("Can't found order with id: " + orderId));
		
		existingOrder.setFullName(orderDTO.getFullName());
		existingOrder.setEmail(orderDTO.getEmail());
		existingOrder.setPhoneNumber(orderDTO.getPhoneNumber());
		existingOrder.setAddress(orderDTO.getAddress());
		existingOrder.setNote(orderDTO.getNote());
		existingOrder.setTotalMoney(orderDTO.getTotalMoney());
		existingOrder.setShippingMethod(orderDTO.getShippingMethod());
		existingOrder.setShippingDate(orderDTO.getShippingDate());
		existingOrder.setShippingAddress(orderDTO.getShippingAddress());
		existingOrder.setPaymentMethod(orderDTO.getPaymentMethod());
		orderRepository.save(existingOrder);

		return mapOrderToOrderResponse(existingOrder);
	}

	@Override
	public List<OrderResponse> getAllOrders() {
		
		List<Order> existingOrders = orderRepository.findAll();
		
		List<OrderResponse> existingOrderResponses = new ArrayList<>();
		
		for (Order order : existingOrders) {
			OrderResponse orderResponse = mapOrderToOrderResponse(order);
			existingOrderResponses.add(orderResponse);
		}
		
		return existingOrderResponses;
	}

	@Override
	public void deleteOrder(Long orderId) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public List<OrderResponse> findByUserId(Long userId) {
		
		List<Order> existingOrders = orderRepository.findByUser_Id(userId);
		List<OrderResponse> existingOrderResponses = new ArrayList<>();
		
		for (Order order : existingOrders) {
			existingOrderResponses.add(mapOrderToOrderResponse(order));
		}
		
		return existingOrderResponses;
	}


	private OrderResponse mapOrderToOrderResponse(Order order) {
		modelMapper.typeMap(Order.class, OrderResponse.class)
			.addMappings(mapper -> mapper.skip(OrderResponse::setUserId));
		OrderResponse existingOrderResponse = new OrderResponse();
		modelMapper.map(order, existingOrderResponse);
    	existingOrderResponse.setUserId(order.getUser().getId());
    	
		return existingOrderResponse;
	}
}
