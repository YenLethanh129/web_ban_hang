package com.project.webbanhang.models;

import java.util.Date;

import jakarta.persistence.*;
import lombok.*;

@Entity
@AllArgsConstructor
@NoArgsConstructor
@Setter
@Getter
@Table(name = "orders")
public class Order extends BaseEntity{
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
    
    @Column(name = "note")
    private String note;
    
    @Column(name = "status")
    private String status;
    
    @Column(name = "total_money")
    private Float totalMoney;
    
    @Column(name = "order_date")
    private Date orderDate;
    
    @Column(name = "shipping_method")
    private String shippingMethod;
    
    @Column(name = "shipping_address")
    private String shippingAddress;
    
    @Column(name = "shipping_date")
    private Date shippingDate;
    
    @Column(name = "tracking_number")
    private String trackingNumber;
    
    @Column(name = "payment_method")
    private String paymentMethod;
    
    @Column(name = "payment_status")
    private String paymentStatus;
    
    @Column(name = "is_active")
    private Boolean isActive;
}