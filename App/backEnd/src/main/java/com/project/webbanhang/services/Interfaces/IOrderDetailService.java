package com.project.webbanhang.services.Interfaces;

import java.util.List;

import com.project.webbanhang.dtos.OrderDetailDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.response.OrderDetailResponse;

public interface IOrderDetailService {

	OrderDetailResponse createOrderDetail(OrderDetailDTO orderDetailDTO);
	
	OrderDetailResponse getOrderDetail(String extractedToken,Long orderDetailId) throws Exception;
	
	List<OrderDetailResponse> getOrderDetails();
	
	List<OrderDetailResponse> getOrderDetailsByOrderId(Long orderId) throws DataNotFoundException;
	
	OrderDetailResponse updateOrderDetail(Long orderDetailId, OrderDetailDTO orderDetailDTO) throws DataNotFoundException;
	
	void deleteOrderDetail(Long orderDetailId);
}
