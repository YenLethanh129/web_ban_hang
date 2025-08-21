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
    @Column(name = "customer_id")
    private Customer customerId;

    @ManyToMany
    @Column(name = "branch_id")
    private Branch branch;

    @Column(name = "total_money")
    private Long totalMoney;

    @OneToMany
    @Column(name = "status_id")
    private OrderStatus status;

    @Column(name = "notes")
    private String notes;
}