package com.project.webbanhang.services;

import java.util.List;

import com.project.webbanhang.dtos.OrderDetailDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.response.OrderDetailResponse;

public interface IOrderDetailService {

	OrderDetailResponse createOrderDetail(OrderDetailDTO orderDetailDTO);
	
	OrderDetailResponse getOrderDetail(Long orderDetailId);
	
	List<OrderDetailResponse> getOrderDetails();
	
	OrderDetailResponse getOrderDetailByOrderId(Long orderId);
	
	OrderDetailResponse updateOrderDetail(Long orderDetailId, OrderDetailDTO orderDetailDTO) throws DataNotFoundException;
	
	void deleteOrderDetail(Long orderDetailId);
}
