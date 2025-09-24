package com.project.webbanhang.repositories;

import com.project.webbanhang.models.orders.OrderShipment;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface OrderShipmentRepository extends JpaRepository<OrderShipment, Long> {
    Optional<OrderShipment> findByOrderId(Long orderId);
}
