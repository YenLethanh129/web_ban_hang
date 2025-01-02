package com.project.webbanhang.services;

import java.util.List;

import com.project.webbanhang.dtos.OrderDetailDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.OrderDetail;

public interface IOrderDetailService {

	OrderDetail createOrderDetail(OrderDetailDTO orderDetailDTO);
	
	OrderDetail getOrderDetail(Long orderDetailId);
	
	List<OrderDetail> getOrderDetails();
	
	OrderDetail getOrderDetailByOrderId(Long orderId);
	
	OrderDetail updateOrderDetail(Long orderDetailId, OrderDetailDTO orderDetailDTO) throws DataNotFoundException;
	
	void deleteOrderDetail(Long orderDetailId);
}
