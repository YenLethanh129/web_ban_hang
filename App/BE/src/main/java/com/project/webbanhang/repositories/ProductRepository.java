package com.project.webbanhang.repositories;

import org.springframework.data.domain.*;
import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.Product;
import com.project.webbanhang.response.ProductResponse;

public interface ProductRepository extends JpaRepository<Product, Long> {
	
	boolean existsByName(String name);
	
	Page<Product> findAll(Pageable pageable);
}
