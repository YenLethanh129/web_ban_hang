package com.project.webbanhang.services.orders;

import java.util.ArrayList;
import java.util.List;

import com.project.webbanhang.services.Interfaces.IOrderDetailService;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.OrderDetailDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.orders.OrderDetail;
import com.project.webbanhang.models.Product;
import com.project.webbanhang.models.orders.Order;
import com.project.webbanhang.repositories.OrderDetailRepository;
import com.project.webbanhang.repositories.OrderRepository;
import com.project.webbanhang.repositories.ProductRepository;
import com.project.webbanhang.response.OrderDetailResponse;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class OrderDetailService implements IOrderDetailService {
	private final OrderDetailRepository orderDetailRepository;
	private final OrderRepository orderRepository;
	private final ProductRepository productRepository;
	
	@Override
	public OrderDetailResponse createOrderDetail(OrderDetailDTO orderDetailDTO) {
		
		OrderDetail orderDetail = mapOrderDetailDTOToOrderDetail(orderDetailDTO);
		orderDetailRepository.save(orderDetail);
		
		return OrderDetailResponse.fromEntity(orderDetail);
	}

	@Override
	public OrderDetailResponse getOrderDetail(Long orderDetailId) {
		
		OrderDetail existingOrderDetail = orderDetailRepository.findById(orderDetailId)
				.orElse(null);
		
		return OrderDetailResponse.fromEntity(existingOrderDetail);
	}

	@Override
	public List<OrderDetailResponse> getOrderDetails() {
		List<OrderDetail> existingOrderDetails = orderDetailRepository.findAll();	
		List<OrderDetailResponse> existingOrderDetailResponses = new ArrayList<>();
		for (OrderDetail orderDetail : existingOrderDetails) {
			existingOrderDetailResponses.add(OrderDetailResponse.fromEntity(orderDetail));
		}
		return existingOrderDetailResponses;
	}

	@Override
	public List<OrderDetailResponse> getOrderDetailsByOrderId(Long orderId) throws DataNotFoundException{
		List<OrderDetail> existingOrderDetails = orderDetailRepository.findByOrder_Id(orderId);
		if (existingOrderDetails.isEmpty()) {
			throw new DataNotFoundException("Can't not found List Order ID");
		}

		List<OrderDetailResponse> listOrderDetailsResponse = new ArrayList<>();
		for (OrderDetail orderDetail : existingOrderDetails) {
			OrderDetailResponse orderDetailResponse = OrderDetailResponse.builder()
					.productName(orderDetail.getProduct().getName())
					.productThumbnail(orderDetail.getProduct().getThumbnail())
					.size(orderDetail.getSize())
					.quantity(orderDetail.getQuantity())
					.totalAmount(orderDetail.getTotalAmount())
					.unitPrice(orderDetail.getUnitPrice())
					.build();
			listOrderDetailsResponse.add(orderDetailResponse);
		}

		return listOrderDetailsResponse;
	}

	@Override
	public OrderDetailResponse updateOrderDetail(Long orderDetailId, OrderDetailDTO orderDetailDTO) throws DataNotFoundException {
		orderDetailRepository.findById(orderDetailId)
			.orElseThrow(() -> new DataNotFoundException("Can't found orderDetail with id: " + orderDetailId));
		
		OrderDetail existingOrderDetail = mapOrderDetailDTOToOrderDetail(orderDetailDTO);
		existingOrderDetail.setId(orderDetailId);
		orderDetailRepository.save(existingOrderDetail);
		
		return OrderDetailResponse.fromEntity(existingOrderDetail);
	}

	@Override
	public void deleteOrderDetail(Long orderDetailId) {
		orderDetailRepository.deleteById(orderDetailId);
	}
	
	private OrderDetail mapOrderDetailDTOToOrderDetail(OrderDetailDTO orderDetailDTO) {
		Order existingOrder = orderRepository.findById(orderDetailDTO.getOrderId())
				.orElse(null);
		Product existingProduct = productRepository.findById(orderDetailDTO.getProductId())
				.orElse(null);
		OrderDetail existingOrderDetail = OrderDetail.builder()
				.order(existingOrder)
				.quantity(orderDetailDTO.getQuantity())
				.product(existingProduct)
				.size(orderDetailDTO.getSize())
				.note(null)
				.totalAmount(orderDetailDTO.getTotalMoney())
				.unitPrice(orderDetailDTO.getUnitPrice())
				.build();
		return existingOrderDetail;
	};

}
