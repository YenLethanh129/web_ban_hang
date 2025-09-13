package com.project.webbanhang.services.Interfaces;

import com.project.webbanhang.models.orders.Order;

public interface IOrderShipmentService {
    void createOrderShipment(Order order, String address);
}
