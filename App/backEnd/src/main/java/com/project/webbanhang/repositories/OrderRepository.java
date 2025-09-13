package com.project.webbanhang.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.orders.Order;

public interface OrderRepository extends JpaRepository<Order, Long>{
    List<Order> findAllByCustomerId(Long customerId);
}
