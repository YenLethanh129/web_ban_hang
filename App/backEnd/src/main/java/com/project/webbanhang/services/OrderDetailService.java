package com.project.webbanhang.services;

import java.util.ArrayList;
import java.util.List;

import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.OrderDetailDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.OrderDetail;
import com.project.webbanhang.models.Product;
import com.project.webbanhang.models.Order;
import com.project.webbanhang.repositories.OrderDetailRepository;
import com.project.webbanhang.repositories.OrderRepository;
import com.project.webbanhang.repositories.ProductRepository;
import com.project.webbanhang.response.OrderDetailResponse;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class OrderDetailService implements IOrderDetailService{
	private final OrderDetailRepository orderDetailRepository;
	private final OrderRepository orderRepository;
	private final ProductRepository productRepository;
	//private final ModelMapper modelMapper;
	
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
	public OrderDetailResponse getOrderDetailByOrderId(Long orderId) {
		
		OrderDetail existingOrderDetail = orderDetailRepository.findByOrder_Id(orderId);
		return OrderDetailResponse.fromEntity(existingOrderDetail);
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
//		modelMapper.typeMap(OrderDetailDTO.class, OrderDetail.class)
//	    	.addMappings(mapper -> {
//	    		mapper.skip(OrderDetail::setId); // Bỏ qua thuộc tính ID
//	    		mapper.skip(OrderDetail::setOrder); // Bỏ qua ánh xạ Order
//	    		mapper.skip(OrderDetail::setProduct); // Bỏ qua ánh xạ Product
//	    });

//		modelMapper.map(orderDetailDTO, existingOrderDetail);
		
		Order order = orderRepository.findById(orderDetailDTO.getOrderId())
				.orElse(null);
		Product product = productRepository.findById(orderDetailDTO.getProductId())
				.orElse(null);
		OrderDetail existingOrderDetail = new OrderDetail();
		
		existingOrderDetail.setOrder(order);
		existingOrderDetail.setProduct(product);
		existingOrderDetail.setColor(orderDetailDTO.getColor());
		existingOrderDetail.setNumberOfProducts(orderDetailDTO.getNumberOfProduct());
		existingOrderDetail.setPrice(orderDetailDTO.getPrice());
		existingOrderDetail.setTotalMoney(orderDetailDTO.getTotalMoney());
		
		return existingOrderDetail;
	};

}
