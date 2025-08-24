package com.project.webbanhang.services;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Optional;

import com.project.webbanhang.models.*;
import com.project.webbanhang.repositories.*;
import org.modelmapper.ModelMapper;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.response.OrderResponse;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class OrderService implements IOrderService{
	private final OrderRepository orderRepository;
	private final CustomerRepository customerRepository;
    private final PaymentMethodRepository paymentMethodRepository;
    private final OrderPaymentRepository orderPaymentRepository;
	private final ModelMapper modelMapper;

    /**
     * Create OrderStatus();
     * Create Order -> getOrderId();
     * Create PaymentMethod();
     * Create PaymentStatus();
     * Create OrderPayment();
     * Return OrderResponse();
     * */
	@Override
	public OrderResponse createOrder(OrderDTO orderDTO) throws DataNotFoundException {
		// tim user
		Customer customer = customerRepository.findById(orderDTO.getUserId())
				.orElseThrow(() -> new DataNotFoundException("Can't not found user with id " + orderDTO.getUserId()));

		OrderStatus orderStatus = OrderStatus.builder()
				.id(1L)
				.status(OrderStatus.PENDING)
				.build();

		Order order = Order.builder()
				.customer(customer)
				.notes(orderDTO.getNote())
				.branch(null)
				.totalMoney((long) orderDTO.getTotalMoney())
				.status(orderStatus)
				.build();

		Order existsOrderRepository = orderRepository.save(order);

        Optional<PaymentMethod> existsPaymentMethod = paymentMethodRepository.findByName(orderDTO.getPaymentMethod());
        if (existsPaymentMethod.isEmpty()) {
            throw new DataNotFoundException("Can't not found Payment Method");
        }

        PaymentStatus paymentStatus = PaymentStatus.builder()
                .id(1L)
                .status(PaymentStatus.PENDING)
                .build();

        OrderPayment orderPayment = OrderPayment.builder()
                .order(existsOrderRepository)
                .paymentMethod(existsPaymentMethod.get())
                .paymentStatus(paymentStatus)
                .amount(existsOrderRepository.getTotalMoney())
                .paymentDate(LocalDateTime.now())
                .transactionId(null)
                .notes(existsOrderRepository.getNotes())
                .build();

        orderPaymentRepository.save(orderPayment);
		
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
	public List<OrderResponse> findByUserId(Long userId) {
		
//		List<Order> existingOrders = orderRepository.findByUser_Id(userId);
		List<OrderResponse> existingOrderResponses = new ArrayList<>();
//
//		for (Order order : existingOrders) {
//			existingOrderResponses.add(mapOrderToOrderResponse(order));
//		}
		
		return existingOrderResponses;
	}

    /**
     * @param order
     * getShippingMethod()
     * getOrderPayment()
     * @return orderResponse
     */
	private OrderResponse mapOrderToOrderResponse(Order order) {
//		modelMapper.typeMap(Order.class, OrderResponse.class)
//			.addMappings(mapper -> {
//				mapper.map(Order::getId, OrderResponse::setOrderId);
//			});
//		OrderResponse existingOrderResponse = new OrderResponse();
//		modelMapper.map(order, existingOrderResponse);

        Optional<OrderPayment> existingOrderPayement = orderPaymentRepository.findByOrderId(order.getId());

        OrderResponse existingOrderResponse = OrderResponse.builder()
                .orderId(order.getId())
                .note(order.getNotes())
                .totalMoney(order.getTotalMoney())
                .status(order.getStatus().getStatus())
                .orderDate(order.getCreatedAt())
                .shippingMethod("STANDARD")
                .paymentMethod(existingOrderPayement.get().getPaymentMethod().getName())
                .paymentStatus(existingOrderPayement.get().getPaymentStatus().getStatus())
                .createdAt(order.getCreatedAt())
                .lastModified(order.getLastModified())
                .build();

		return existingOrderResponse;
	}
}
