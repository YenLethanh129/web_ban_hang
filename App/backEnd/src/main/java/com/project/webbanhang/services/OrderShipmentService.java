package com.project.webbanhang.services;

import com.project.webbanhang.models.Order;
import com.project.webbanhang.models.OrderShipment;
import com.project.webbanhang.models.ShippingProvider;
import com.project.webbanhang.repositories.OrderShipmentRepository;
import com.project.webbanhang.repositories.ShippingProviderRepository;
import com.project.webbanhang.services.Interfaces.IOrderShipmentService;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class OrderShipmentService implements IOrderShipmentService {
    private final OrderShipmentRepository orderShipmentRepository;
    private final ShippingProviderRepository shippingProviderRepository;

    @Override
    public void createOrderShipment(Order order, String address) {
        LocalDateTime estimateDelivery;
        if (order.getCreatedAt() != null) {
            estimateDelivery = order.getCreatedAt().plusHours(1);
        } else {
            estimateDelivery = null;
        }

        Optional<ShippingProvider> shippingProvider = shippingProviderRepository.findById(1L);

        OrderShipment orderShipment = OrderShipment.builder()
                .order(order)
                .shippingAddress(address)
                .shippingCost(0L)
                .shippingMethod("Giao hàng nhanh")
                .estimatedDeliveryDate(estimateDelivery)
                .notes(order.getNotes())
                .build();

        shippingProvider.ifPresent(orderShipment::setShippingProvider);

        orderShipmentRepository.save(orderShipment);
    }
}
