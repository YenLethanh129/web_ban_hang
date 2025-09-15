package com.project.webbanhang.models;

import jakarta.persistence.*;
import lombok.*;

/**
 1	PENDING
 2	CONFIRMED
 3	PROCESSING
 4	SHIPPED
 5	DELIVERED
 6	CANCELLED
 7	RETURNED
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

    private String name;

    /** Đang chờ (thanh toán) */
    public static String PENDING = "PENDING";

    /** Xác nận đơn hàng */
    public static String CONFIRMED = "CONFIRMED";

    /** Đang xử lý (làm đơn) */
    public static String PROCESSING = "PROCESSING";

    /** Đã rời khỏi cửa hàng */
    public static String SHIPPED = "SHIPPED";

    /** Đã giao cho người nhận */
    public static String DELIVERED = "DELIVERED";

    /** Đơn hàng đã bi hủy */
    public static String CANCELLED = "CANCELLED";

    /** Trả hàng */
    public static String RETURNED = "RETURNED";
}
