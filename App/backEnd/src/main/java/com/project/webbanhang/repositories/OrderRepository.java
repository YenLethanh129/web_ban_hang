package com.project.webbanhang.repositories;

import java.util.List;
import java.util.Optional;

import com.project.webbanhang.models.OrderPayment;
import com.project.webbanhang.response.OrderResponse;
import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.Order;

public interface OrderRepository extends JpaRepository<Order, Long>{
    List<Order> findAllByCustomerId(Long customerId);
}
