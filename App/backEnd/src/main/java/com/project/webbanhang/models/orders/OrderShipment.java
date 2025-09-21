package com.project.webbanhang.models.orders;

import com.project.webbanhang.models.BaseEntity;
import com.project.webbanhang.models.ShippingProvider;
import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDateTime;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
@Table(name = "order_shipments")
public class OrderShipment extends BaseEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "order_id")
    private Order order;

    @ManyToOne
    @JoinColumn(name = "shipping_provider_id")
    private ShippingProvider shippingProvider;

    @Column(name = "shipping_address")
    private String shippingAddress;

    @Column(name = "shipping_cost")
    private Long shippingCost;

    @Column(name = "shipping_method")
    private String shippingMethod;

    @Column(name = "estimated_delivery_date")
    private LocalDateTime estimatedDeliveryDate;

    @Column(name = "notes")
    private String notes;
}
