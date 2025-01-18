package com.project.webbanhang.services;

import com.project.webbanhang.dtos.ProductImageDTO;
import com.project.webbanhang.models.ProductImage;


public interface IProductImageService {

	public ProductImage createProductImage(ProductImageDTO productDTO);
	
	ProductImage updateProductImage(Long id, ProductImageDTO productDTO);
	
	void deleteProductImages(Long id);
}
