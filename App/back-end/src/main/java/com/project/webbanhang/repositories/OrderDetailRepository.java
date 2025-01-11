package com.project.webbanhang.repositories;

import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.OrderDetail;

public interface OrderDetailRepository extends JpaRepository<OrderDetail, Long>{

	OrderDetail findByOrder_Id(Long orderId);
}
