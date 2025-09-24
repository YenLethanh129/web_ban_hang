package com.project.webbanhang.services;

import java.util.Optional;

import com.project.webbanhang.services.Interfaces.IProductImageService;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.ProductImageDTO;
import com.project.webbanhang.models.Product;
import com.project.webbanhang.models.ProductImage;
import com.project.webbanhang.repositories.ProductImageRepository;
import com.project.webbanhang.repositories.ProductRepository;

import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class ProductImageService implements IProductImageService {

	private final ProductRepository productRepository;
	private final ProductImageRepository productImageRepository;
	
	@Override
	@Transactional
	public ProductImage createProductImage(ProductImageDTO productDTO) {
		Product product = new Product();
		Optional<Product> existingProduct = productRepository.findById(productDTO.getProductId());
		if (existingProduct.isPresent()) {
			product = existingProduct.get();
		}
		
		ProductImage productImage = ProductImage.builder()
				.imageUrl(productDTO.getImageUrl())
				.product(product)
				.build();
		return productImageRepository.save(productImage);
	}

	@Override
	public ProductImage updateProductImage(Long id, ProductImageDTO productDTO) {
		return null;
	}

	@Override
	public void deleteProductImages(Long id) {
	}

}
