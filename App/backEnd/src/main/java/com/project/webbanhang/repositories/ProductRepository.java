package com.project.webbanhang.repositories;

import org.springframework.data.domain.*;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

import com.project.webbanhang.models.Product;

public interface ProductRepository extends JpaRepository<Product, Long> {
	
	boolean existsByName(String name);
	
	Page<Product> findAll(Pageable pageable);
	
	Page<Product> findByCategoryId(Long categoryId, Pageable pageable);
}
