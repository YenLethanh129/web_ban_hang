package com.project.webbanhang.services;

import com.project.webbanhang.models.Order;

public interface IOrderShipmentService {
    void createOrderShipment(Order order, String address);
}
