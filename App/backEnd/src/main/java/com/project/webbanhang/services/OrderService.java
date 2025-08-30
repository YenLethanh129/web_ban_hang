package com.project.webbanhang.services;

import java.time.LocalDateTime;
import java.util.*;

import com.project.webbanhang.models.*;
import com.project.webbanhang.repositories.*;
import com.project.webbanhang.utils.TrackingNumberGenerator;
import org.modelmapper.ModelMapper;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.response.OrderResponse;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class OrderService implements IOrderService{
	private final IOrderPaymentService orderPaymentService;
	private final IOrderDeliveryTrackingService orderDeliveryTrackingService;
	private final IOrderShipmentService orderShipmentService;

	private final OrderRepository orderRepository;
	private final CustomerRepository customerRepository;
    private final OrderPaymentRepository orderPaymentRepository;

	/**
	 * create OrderDeliveryTracking
	 * create OrderPayment
	 * create OrderShipment
	 * */
	@Override
	public OrderResponse createOrder(OrderDTO orderDTO) throws Exception {
		// tim user
		Customer customer = customerRepository.findById(orderDTO.getUserId())
				.orElseThrow(() -> new DataNotFoundException("Can't not found user with id " + orderDTO.getUserId()));
		OrderStatus orderStatus = OrderStatus.builder()
				.id(1L)
				.name(OrderStatus.PENDING)
				.build();
		String orderUUID = UUID.randomUUID().toString();
		String orderCode = "ORD" + TrackingNumberGenerator.generateRandomTrackingNumber(3);

		Order order = Order.builder()
				.orderUUID(orderUUID)
				.orderCode(orderCode)
				.customer(customer)
				.notes(orderDTO.getNote())
				.branch(null)
				.totalMoney((long) orderDTO.getTotalMoney())
				.status(orderStatus)
				.build();

		Order existsOrderRepository = orderRepository.save(order);

		orderDeliveryTrackingService.createOrderDeliveryTrackingService(existsOrderRepository);
		orderPaymentService.createOrderPayment(existsOrderRepository, orderDTO.getPaymentMethod());
		orderShipmentService.createOrderShipment(existsOrderRepository, orderDTO.getAddress());

		return mapOrderToOrderResponse(existsOrderRepository);
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
		
//		existingOrder.setFullName(orderDTO.getFullName());
//		existingOrder.setEmail(orderDTO.getEmail());
//		existingOrder.setPhoneNumber(orderDTO.getPhoneNumber());
//		existingOrder.setAddress(orderDTO.getAddress());
//		existingOrder.setNote(orderDTO.getNote());
//		existingOrder.setTotalMoney(orderDTO.getTotalMoney());
//		existingOrder.setShippingMethod(orderDTO.getShippingMethod());
//		existingOrder.setShippingDate(orderDTO.getShippingDate());
//		existingOrder.setShippingAddress(orderDTO.getShippingAddress());
//		existingOrder.setPaymentMethod(orderDTO.getPaymentMethod());
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
	public void deleteOrder(Long orderId) throws DataNotFoundException {
		Order existingOrder = orderRepository.findById(orderId)
				.orElseThrow(() -> new DataNotFoundException("Can't found order with id: " + orderId));
//		existingOrder.setIsActive(false);
		orderRepository.save(existingOrder);
	}

	@Override
	public List<OrderResponse> findAllByCustomerId(Long userId) {
		List<Order> existingOrder = orderRepository.findAllByCustomerId(userId);
		List<OrderResponse> existingOrderResponses = new ArrayList<>();
		for (Order order : existingOrder) {
			OrderResponse orderResponse = mapOrderToOrderResponse(order);
			existingOrderResponses.add(orderResponse);
		}
		return existingOrderResponses;
	}

    /**
     * @param order
     * getShippingMethod()
     * getOrderPayment()
     * @return orderResponse
     */
	public OrderResponse mapOrderToOrderResponse(Order order) {
        Optional<OrderPayment> existingOrderPayement = orderPaymentRepository.findByOrderId(order.getId());

        OrderResponse existingOrderResponse = OrderResponse.builder()
                .orderId(order.getId())
                .note(order.getNotes())
                .totalMoney(order.getTotalMoney())
                .status(order.getStatus().getName())
                .orderDate(order.getCreatedAt())
                .shippingMethod("STANDARD")
                .paymentMethod(existingOrderPayement.get().getPaymentMethod().getName())
                .paymentStatus(existingOrderPayement.get().getPaymentStatus().getName())
                .createdAt(order.getCreatedAt())
                .lastModified(order.getLastModified())
                .build();

		return existingOrderResponse;
	}
}
