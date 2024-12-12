package com.project.webbanhang.repositories;

import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.Order;

public interface OrderRepository extends JpaRepository<Order, Long>{

}
