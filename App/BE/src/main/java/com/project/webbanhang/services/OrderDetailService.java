package com.project.webbanhang.services;

import java.util.List;

import org.modelmapper.ModelMapper;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.OrderDetailDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.OrderDetail;
import com.project.webbanhang.models.Product;
import com.project.webbanhang.models.Order;
import com.project.webbanhang.repositories.OrderDetailRepository;
import com.project.webbanhang.repositories.OrderRepository;
import com.project.webbanhang.repositories.ProductRepository;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class OrderDetailService implements IOrderDetailService{
	private final OrderDetailRepository orderDetailRepository;
	private final OrderRepository orderRepository;
	private final ProductRepository productRepository;
	//private final ModelMapper modelMapper;
	
	@Override
	public OrderDetail createOrderDetail(OrderDetailDTO orderDetailDTO) {
		
		OrderDetail orderDetail = mapOrderDetailDTOToOrderDetail(orderDetailDTO);
		orderDetailRepository.save(orderDetail);
		
		return orderDetail;
	}

	@Override
	public OrderDetail getOrderDetail(Long orderDetailId) {
		
		OrderDetail existingOrderDetail = orderDetailRepository.findById(orderDetailId)
				.orElse(null);
		
		return existingOrderDetail;
	}

	@Override
	public List<OrderDetail> getOrderDetails() {
		
		List<OrderDetail> existingOrderDetails = orderDetailRepository.findAll();		
		return existingOrderDetails;
	}

	@Override
	public OrderDetail getOrderDetailByOrderId(Long orderId) {
		
		OrderDetail existingOrderDetail = orderDetailRepository.findByOrder_Id(orderId);
		return existingOrderDetail;
	}

	@Override
	public OrderDetail updateOrderDetail(Long orderDetailId, OrderDetailDTO orderDetailDTO) throws DataNotFoundException {
		
		orderDetailRepository.findById(orderDetailId)
			.orElseThrow(() -> new DataNotFoundException("Can't found orderDetail with id: " + orderDetailId));
		
		OrderDetail orderDetail = mapOrderDetailDTOToOrderDetail(orderDetailDTO);
		orderDetail.setId(orderDetailId);
		orderDetailRepository.save(orderDetail);
		
		return orderDetail;
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
