package com.project.webbanhang.services;

import java.security.InvalidParameterException;
import java.util.Optional;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.ProductDTO;
import com.project.webbanhang.dtos.ProductImageDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.exceptions.InvalidParamException;
import com.project.webbanhang.models.Category;
import com.project.webbanhang.models.Product;
import com.project.webbanhang.models.ProductImage;
import com.project.webbanhang.repositories.CategoryRepository;
import com.project.webbanhang.repositories.ProductRepository;
import com.project.webbanhang.repositories.ProductmageRepository;
import com.project.webbanhang.response.ProductResponse;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class ProductService implements IProductService{
	
	private final ProductRepository productRepository;
	private final CategoryRepository categoryRepository;
	private final ProductmageRepository productImageRepository;

	@Override
	public Product createProduct(ProductDTO productDTO) throws DataNotFoundException {
		Category existingCategory = categoryRepository
				.findById(productDTO.getCategoryId())
				.orElseThrow(() -> new DataNotFoundException("Can't found category with id: " + productDTO.getCategoryId()));
		
		Product newProduct = Product.builder()
				.name(productDTO.getName())
				.price(productDTO.getPrice())
				.thumbnail(productDTO.getThumbnail())
				.category(existingCategory)
				.build();
		return productRepository.save(newProduct);
	}

	@Override
	public Product getProductById(Long id) throws DataNotFoundException {
		return productRepository.findById(id)
				.orElseThrow(() -> new DataNotFoundException("Can't found product with id " + id));
	}

	@Override
	public Page<ProductResponse> getAllProducts(PageRequest pageRequest) {
		// TODO Auto-generated method stub
		return productRepository.findAll(pageRequest).map(product -> {
			ProductResponse productResponse = ProductResponse.builder()
					.name(product.getName())
					.price(product.getPrice())
					.thumbnail(product.getThumbnail())
					.description(product.getDescription())
					.categoryId(product.getCategory().getId())
					.build();
		});
	}

	@Override
	public Product updateProduct(Long id, ProductDTO productDTO) throws DataNotFoundException {
		Product existingProduct = getProductById(productDTO.getCategoryId());
		
		Category existingCategory = categoryRepository
				.findById(productDTO.getCategoryId())
				.orElseThrow(() -> new DataNotFoundException("Can't found category with id: " + productDTO.getCategoryId()));
		
		existingProduct.setName(productDTO.getName());
		existingProduct.setCategory(existingCategory);
		existingProduct.setDescription(productDTO.getDescription());
		existingProduct.setThumbnail(productDTO.getThumbnail());
		existingProduct.setPrice(productDTO.getPrice());
		
		return productRepository.save(existingProduct);
	}

	@Override
	public void deleteProduct(Long id) {
		Optional<Product> optionalProduct = productRepository.findById(id);
		optionalProduct.ifPresent(productRepository::delete);
	}

	@Override
	public boolean existsByName(String nameProduct) {
		return existsByName(nameProduct);
	}
	
	@Override
	public ProductImage createProductImage(Long productId, ProductImageDTO productImageDTO) throws DataNotFoundException, InvalidParamException {
		Product existingProduct = productRepository.findById(productId)
				.orElseThrow(() -> new DataNotFoundException("Can't found product with id " + productId));
		
		ProductImage newProductImage = ProductImage.builder()
				.product(existingProduct)
				.imageUrl(productImageDTO.getImageUrl())
				.build();
		
		int size = productImageRepository.findByProductId(productId).size();
		if (size >= 5) {
			throw new InvalidParamException("This product had 5 images, delete some it if you want push more!");
		}
		return productImageRepository.save(newProductImage);
	}

}
