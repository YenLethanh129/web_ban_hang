package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.SuperBuilder;

/**
 * 1	PENDING
 * 2	IN_TRANSIT
 * 3	OUT_FOR_DELIVERY
 * 4	DELIVERED
 * 5	DELIVERY_FAILED
 * 6	RETURNED
 */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@SuperBuilder
@Table(name = "delivery_statuses")
public class DeliveryStatus {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "name")
    private String name;

    public static String PENDING = "PENDING";
    public static String IN_TRANSIT = "IN_TRANSIT";
    public static String OUT_FOR_DELIVERY = "OUT_FOR_DELIVERY";
    public static String DELIVERED = "DELIVERED";
    public static String DELIVERY_FAILED = "DELIVERY_FAILED";
    public static String RETURNED = "RETURNED";
}
