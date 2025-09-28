package com.project.webbanhang.services.Interfaces;

import com.project.webbanhang.models.orders.Order;
import com.project.webbanhang.models.orders.OrderShipment;

import java.util.Optional;

public interface IOrderShipmentService {
    void createOrderShipment(Order order, String address);

    Optional<OrderShipment> getOrderShipmentByOrderId(Long orderId);
}
