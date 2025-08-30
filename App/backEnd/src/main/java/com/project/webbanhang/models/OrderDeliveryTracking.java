package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.SuperBuilder;

import java.time.LocalDateTime;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
@Table(name = "order_delivery_tracking")
public class OrderDeliveryTracking extends BaseEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "order_id")
    private Order order;

    @Column(name = "tracking_number")
    private String trackingNumber;

    @ManyToOne
    @JoinColumn(name = "status_id")
    private DeliveryStatus status;

    @Column(name = "location")
    private String location;

    @Column(name = "estimated_delivery")
    private LocalDateTime estimatedDelivery;

    @ManyToOne
    @JoinColumn(name = "delivery_person_id")
    private Employee deliveryPerson;

    @ManyToOne
    @JoinColumn(name = "shipping_provider_id")
    private ShippingProvider shippingProvider;
}
