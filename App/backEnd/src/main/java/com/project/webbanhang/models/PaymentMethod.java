package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;

/**
 * 1 MOMO
 */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
@Table(name = "payment_methods")
public class PaymentMethod {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "name")
    private String name;

    public static String MOMO = "MOMO";
}
