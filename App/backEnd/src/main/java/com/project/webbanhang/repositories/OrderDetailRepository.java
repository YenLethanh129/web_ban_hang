package com.project.webbanhang.repositories;

import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.orders.OrderDetail;

import java.util.List;

public interface OrderDetailRepository extends JpaRepository<OrderDetail, Long>{

	List<OrderDetail> findByOrder_Id(Long orderId);
}
