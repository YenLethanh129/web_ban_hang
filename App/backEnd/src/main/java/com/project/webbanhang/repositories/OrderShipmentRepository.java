package com.project.webbanhang.repositories;

import com.project.webbanhang.models.orders.OrderShipment;
import org.springframework.data.jpa.repository.JpaRepository;

public interface OrderShipmentRepository extends JpaRepository<OrderShipment, Long> {
}
