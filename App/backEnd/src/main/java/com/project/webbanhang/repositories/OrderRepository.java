package com.project.webbanhang.repositories;

import java.util.List;

import feign.Param;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.orders.Order;
import org.springframework.data.jpa.repository.Query;

public interface OrderRepository extends JpaRepository<Order, Long>{
    List<Order> findAllByCustomerId(Long customerId);
}
