package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Table(name = "payment_statuses")
public class PaymentStatus {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "status")
    private String status;

    // Đang chờ xử lý
    public static String PENDING = "PENDING";

    // Đã thanh toán
    public static String PAID = "PAID";

    // Hoàn trả
    public static String REFUNDED = "REFUNDED";

    // Vô hiệu
    public static String VOIDED = "VOIDED";
}
