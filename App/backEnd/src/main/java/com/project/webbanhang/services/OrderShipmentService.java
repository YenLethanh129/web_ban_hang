package com.project.webbanhang.services;

import com.project.webbanhang.models.Order;
import com.project.webbanhang.models.OrderShipment;
import com.project.webbanhang.repositories.OrderShipmentRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;

@Service
@RequiredArgsConstructor
public class OrderShipmentService implements IOrderShipmentService{
    private final OrderShipmentRepository orderShipmentRepository;

    @Override
    public void createOrderShipment(Order order, String address) {
        LocalDateTime estimateDelivery;
        if (order.getCreatedAt() != null) {
            estimateDelivery = order.getCreatedAt().plusHours(1);
        } else {
            estimateDelivery = null;
        }

        OrderShipment orderShipment = OrderShipment.builder()
                .order(order)
                .shippingProvider(null)
                .shippingAddress(address)
                .shippingCost(0L)
                .shippingMethod("Giao hàng nhanh")
                .estimatedDeliveryDate(estimateDelivery)
                .notes(order.getNotes())
                .build();

        orderShipmentRepository.save(orderShipment);
    }
}
