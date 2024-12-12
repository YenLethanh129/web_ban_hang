package com.project.webbanhang.models;

import java.time.LocalDateTime;
import java.util.Date;

import jakarta.persistence.*;
import lombok.*;

@Entity
@Data
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Table(name = "orders")
public class Order {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;
    
    @Column(name = "fullname", length = 100)
    private String fullName;
    
    @Column(name = "email", length = 100)
    private String email;
    
    @Column(name = "phone_number", nullable = false, length = 20)
    private String phoneNumber;
    
    @Column(name = "address", nullable = false, length = 200)
    private String address;
    
    @Column(name = "status")
    private String status;
    
    @Column(name = "total_money")
    private Float totalMoney;
    
    @Column(name = "order_date")
    private LocalDateTime orderDate;
    
    @Column(name = "shippng_method")
    private String shippingMethod;
    
    @Column(name = "shipping_address")
    private Date shippingAddress;
    
    @Column(name = "shipping_date")
    private Date shippingDate;
    
    @Column(name = "tracking_number")
    private String trackingNumber;
    
    @Column(name = "payment_method")
    private String paymentMethod;
    
    @Column(name = "payment_status")
    private String paymentStatus;
    
    @Column(name = "payment_date")
    private Date paymentDate;
    
    @Column(name = "active")
    private Boolean active;
}