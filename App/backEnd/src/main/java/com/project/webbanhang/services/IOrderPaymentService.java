package com.project.webbanhang.services;

import com.project.webbanhang.models.Order;

public interface IOrderPaymentService {
    void createOrderPayment(Order order, String paymentMethod) throws Exception;
}
