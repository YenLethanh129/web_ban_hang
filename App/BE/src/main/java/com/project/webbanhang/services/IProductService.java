package com.project.webbanhang.services;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.ProductDTO;
import com.project.webbanhang.dtos.ProductImageDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.exceptions.InvalidParamException;
import com.project.webbanhang.models.Product;
import com.project.webbanhang.models.ProductImage;


public interface IProductService {

	public Product createProduct(ProductDTO productDTO) throws DataNotFoundException;
	
	Product getProductById(Long id) throws DataNotFoundException;
	
	Page<Product> getAllProducts(PageRequest pageRequest);
	
	Product updateProduct(Long id, ProductDTO productDTO) throws DataNotFoundException;
	
	void deleteProduct(Long id);
	
	boolean existsByName(String nameProduct);
	
	public ProductImage createProductImage(Long productId, ProductImageDTO productImageDTO) throws DataNotFoundException, InvalidParamException;
}
