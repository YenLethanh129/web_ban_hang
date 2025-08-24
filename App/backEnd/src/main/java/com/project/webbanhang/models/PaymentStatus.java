package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;

/**
 * (1, 'PENDING'),
 * (2, 'PAID'),
 * (3, 'REFUNDED'),
 * (4, 'VOIDED');
 * */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
@Table(name = "payment_statuses")
public class PaymentStatus {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "status")
    private String status;

    /**
     * Đang chờ xử lý
     */
    public static String PENDING = "PENDING";

    /**
     * Đã Thanh Toán
     */
    public static String PAID = "PAID";

    /**
     * Hoàn trả đơn hàng
     */
    public static String REFUNDED = "REFUNDED";

    /**
     * Vô hiệu hóa đơn hàng
     */
    public static String VOIDED = "VOIDED";
}
