package com.project.webbanhang.repositories;

import com.project.webbanhang.models.orders.OrderDeliveryTracking;
import org.springframework.data.jpa.repository.JpaRepository;

public interface OrderDeliveryTrackingRepository extends JpaRepository<OrderDeliveryTracking, Long> {
}
