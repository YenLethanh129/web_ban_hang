package com.project.webbanhang.repositories;

import com.project.webbanhang.models.orders.OrderStatus;
import org.springframework.data.jpa.repository.JpaRepository;

public interface OrderStatusRepository extends JpaRepository<OrderStatus, Long> {
}
