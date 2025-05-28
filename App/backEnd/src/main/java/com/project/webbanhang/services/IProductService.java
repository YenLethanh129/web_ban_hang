package com.project.webbanhang.services;

import java.util.List;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;

import com.project.webbanhang.dtos.ProductDTO;
import com.project.webbanhang.dtos.ProductImageDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.exceptions.InvalidParamException;
import com.project.webbanhang.models.Product;
import com.project.webbanhang.models.ProductImage;
import com.project.webbanhang.response.ProductResponse;


public interface IProductService {

	public Product createProduct(ProductDTO productDTO) throws DataNotFoundException;
	
	Product getProductById(Long id) throws DataNotFoundException;
	
	Page<ProductResponse> getProductsByCategoryId(Long categoryId, PageRequest pageable) throws DataNotFoundException;
	
	Page<ProductResponse> getAllProducts(PageRequest pageable);
	
	Product updateProduct(Long id, ProductDTO productDTO) throws DataNotFoundException;
	
	void deleteProduct(Long id);
	
	boolean existsByName(String nameProduct);
	
	public ProductImage createProductImage(Long productId, ProductImageDTO productImageDTO) throws DataNotFoundException, InvalidParamException;
}
