package com.project.webbanhang.services.orders;

import java.util.*;

import com.project.webbanhang.models.*;
import com.project.webbanhang.models.orders.Order;
import com.project.webbanhang.models.orders.OrderPayment;
import com.project.webbanhang.models.orders.OrderStatus;
import com.project.webbanhang.repositories.*;
import com.project.webbanhang.response.OrderDetailResponse;
import com.project.webbanhang.services.Interfaces.*;
import com.project.webbanhang.utils.TrackingNumberGenerator;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.OrderDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.response.OrderResponse;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class OrderService implements IOrderService {
	private final IOrderPaymentService orderPaymentService;
	private final IOrderDeliveryTrackingService orderDeliveryTrackingService;
	private final IOrderShipmentService orderShipmentService;
	private final IUserService userService;
	private final IOrderDetailService orderDetailService;

	private final OrderRepository orderRepository;
	private final CustomerRepository customerRepository;
    private final OrderPaymentRepository orderPaymentRepository;

	/**
	 * TOP 10 OWASP 2023
	 * API1:2023 - Broken Object Level Authorization (BOLA)
	 * Thay vì truyền userId từ client, ta sẽ lấy userId từ token
	 * Điều này ngăn chặn việc hacker có thể lấy id của người dùng khác và truy cập vào thông tin cá nhân của họ
	 * */
	/**
	 * TOP 10 OWASP 2023
	 * API3: 2023 - Broken Object Property Level Authorization
	 * Hacker có thể truy cập vào các thuộc tính nhạy cảm của người dùng khác
	 * Giải pháp: Sử dụng Response để chỉ trả về các thuộc tính cần thiết
	 * */
	@Override
	public OrderResponse createOrder(String token, OrderDTO orderDTO) throws Exception {
		// tim user
		User user = userService.getUserProfileFromToken(token);
		Customer customer = customerRepository.findById(user.getId())
				.orElseThrow(() -> new DataNotFoundException("Can't not found user with id " + user.getId()));
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
		orderRepository.save(existingOrder);

		return mapOrderToOrderResponse(existingOrder);
	}

	/**
	 * TOP 10 OWASP 2023
	 * API3: 2023 - Broken Object Property Level Authorization
	 * Hacker có thể truy cập vào các thuộc tính nhạy cảm của người dùng khác
	 * Giải pháp: Sử dụng Response để chỉ trả về các thuộc tính cần thiết
	 * */
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

	/**
	 * TOP 10 OWASP 2023
	 * API3: 2023 - Broken Object Property Level Authorization
	 * Hacker có thể truy cập vào các thuộc tính nhạy cảm của người dùng khác
	 * Giải pháp: Sử dụng Response để chỉ trả về các thuộc tính cần thiết
	 * */
	@Override
	public List<OrderResponse> findByCustomer(String token) throws Exception {
		User user = userService.getUserProfileFromToken(token);

		List<Order> existingOrder = orderRepository.findAllByCustomerId(user.getId());

		existingOrder.sort((o1, o2) -> o2.getCreatedAt().compareTo(o1.getCreatedAt()));
		List<OrderResponse> existingOrderResponses = new ArrayList<>();
		for (Order order : existingOrder) {
			OrderResponse orderResponse = mapOrderToOrderResponse(order);

			List<OrderDetailResponse> existingOrderDetails = orderDetailService.getOrderDetailsByOrderId(order.getId());
			if (!existingOrderDetails.isEmpty()) {
				orderResponse.setOrderDetails(existingOrderDetails);
			}

			existingOrderResponses.add(orderResponse);
			if (existingOrderResponses.size() >= 5) {
				return existingOrderResponses;
			}
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
				.orderUUID(order.getOrderUUID())
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
