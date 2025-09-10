package com.project.webbanhang.services;

import com.project.webbanhang.models.DeliveryStatus;
import com.project.webbanhang.models.Order;
import com.project.webbanhang.models.OrderDeliveryTracking;
import com.project.webbanhang.models.ShippingProvider;
import com.project.webbanhang.repositories.OrderDeliveryTrackingRepository;
import com.project.webbanhang.repositories.ShippingProviderRepository;
import com.project.webbanhang.services.Interfaces.IOrderDeliveryTrackingService;
import com.project.webbanhang.utils.TrackingNumberGenerator;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class OrderDeliveryTrackingService implements IOrderDeliveryTrackingService {
    private final OrderDeliveryTrackingRepository orderDeliveryTrackingRepository;
    private final ShippingProviderRepository shippingProviderRepository;

    @Override
    public void createOrderDeliveryTrackingService(Order order) {
        String trackingNumber = "SPF" + TrackingNumberGenerator.generateRandomTrackingNumber(8);
        DeliveryStatus deliveryStatus = DeliveryStatus.builder()
                .id(1L)
                .name(DeliveryStatus.PENDING)
                .build();
        LocalDateTime estimateDelivery;
        if (order.getCreatedAt() != null) {
            estimateDelivery = order.getCreatedAt().plusHours(1);
        } else {
            estimateDelivery = null;
        }

        Optional<ShippingProvider> shippingProvider = shippingProviderRepository.findById(1L);

        OrderDeliveryTracking orderDeliveryTracking = OrderDeliveryTracking.builder()
                .order(order)
                .trackingNumber(trackingNumber)
                .status(deliveryStatus)
                .location("29A Hau Giang")
                .estimatedDelivery(estimateDelivery)
                .deliveryPerson(null)
                .build();

        shippingProvider.ifPresent(orderDeliveryTracking::setShippingProvider);

        orderDeliveryTrackingRepository.save(orderDeliveryTracking);
    }
}
