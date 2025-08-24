package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;

/**
 1	PENDING
 2	PROCESSING
 3	IN_TRANSIT
 4	OUT_FOR_DELIVERY
 5	DELIVERED
 6	DELIVERY_FAILED
 * */
@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Builder
@Table(name = "order_statuses")
public class OrderStatus {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String status;

    /** Đang chờ (thanh toán) */
    public static String PENDING = "PENDING";

    /** Đang xử lý (làm đơn) */
    public static String PROCESSING = "PROCESSING";

    /** Đã rời khỏi cửa hàng */
    public static String IN_TRANSIT = "IN_TRANSIT";

    /** Đã đến địa chỉ nhận hàng */
    public static String OUT_FOR_DELIVERY = "OUT_FOR_DELIVERY";

    /** Đã giao cho người nhận */
    public static String DELIVERED = "DELIVERED";

    /** Không có ai nhận hàng */
    public static String DELIVERY_FAILED = "DELIVERY_FAILED";
}
