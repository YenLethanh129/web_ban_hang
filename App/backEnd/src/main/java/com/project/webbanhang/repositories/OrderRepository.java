package com.project.webbanhang.repositories;

import java.util.Optional;

import com.project.webbanhang.models.OrderPayment;
import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.Order;

public interface OrderRepository extends JpaRepository<Order, Long>{
}
