package com.project.webbanhang.services.Interfaces;

import com.project.webbanhang.models.orders.Order;

public interface IOrderPaymentService {
    void createOrderPayment(Order order, String paymentMethod) throws Exception;
}
