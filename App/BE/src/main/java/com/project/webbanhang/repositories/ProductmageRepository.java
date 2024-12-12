package com.project.webbanhang.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.ProductImage;

public interface ProductmageRepository extends JpaRepository<ProductImage, Long>{

	List<ProductImage> findByProductId(Long productId);
}
