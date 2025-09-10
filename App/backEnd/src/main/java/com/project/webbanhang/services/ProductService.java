package com.project.webbanhang.services;

import java.util.Optional;

import com.project.webbanhang.response.CacheablePageResponse;
import com.project.webbanhang.services.Interfaces.IProductService;
import jakarta.transaction.Transactional;
import org.springframework.cache.annotation.CacheEvict;
import org.springframework.cache.annotation.CachePut;
import org.springframework.cache.annotation.Cacheable;
import org.springframework.cache.annotation.Caching;
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
import com.project.webbanhang.repositories.ProductImageRepository;
import com.project.webbanhang.response.ProductResponse;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class ProductService implements IProductService {
	
	private final ProductRepository productRepository;
	private final CategoryRepository categoryRepository;
	private final ProductImageRepository productImageRepository;

	// Done
	@Override
	@Transactional
	@CacheEvict(value = { "products", "productsByCategory" }, allEntries = true)
	public Product createProduct(ProductDTO productDTO) throws DataNotFoundException {
		Category existingCategory = categoryRepository
				.findById(productDTO.getCategoryId())
				.orElseThrow(() -> new DataNotFoundException("Can't found category with id: " + productDTO.getCategoryId()));
		
		Product newProduct = Product.builder()
				.name(productDTO.getName())
				.price(productDTO.getPrice())
				.thumbnail(productDTO.getThumbnail())
				.description(productDTO.getDescription())
				.category(existingCategory)
				.tax(null)
				.build();
		return productRepository.save(newProduct);
	}

	// Done
	@Override
	@Cacheable(value = "product", key = "#id")
	public Product getProductById(Long id) throws DataNotFoundException {
		return productRepository.findById(id)
				.orElseThrow(() -> new DataNotFoundException("Can't found product with id " + id));
	}

	// Done
	@Override
	@Cacheable(value = "products", key = "'page:' + #pageRequest.pageNumber + ':limit:' + #pageRequest.pageSize")
	public CacheablePageResponse<ProductResponse> getAllProducts(PageRequest pageRequest) {
		Page<ProductResponse> page = productRepository.findAll(pageRequest)
				.map(ProductResponse::fromEntity);
		return new CacheablePageResponse<>(
				page.getContent(),
				page.getNumber(),
				page.getSize(),
				page.getTotalElements(),
				page.getTotalPages(),
				page.isLast()
		);
	}

	// Done
	@Override
	@Transactional
	@Caching(
		put = @CachePut(value = "product", key = "#id"),
		evict = {
			@CacheEvict(value = "products", allEntries = true),
			@CacheEvict(value = "productsByCategory", allEntries = true)
		}
	)
	public Product updateProduct(Long id, ProductDTO productDTO) throws DataNotFoundException {
		Product existingProduct = getProductById(id);
		
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
	@Transactional
	@Caching(evict = {
		@CacheEvict(value = "product", key = "#id"),
		@CacheEvict(value = "products", allEntries = true),
		@CacheEvict(value = "productsByCategory", allEntries = true)
	})
	public void deleteProduct(Long id) {
		Optional<Product> optionalProduct = productRepository.findById(id);
		optionalProduct.ifPresent(productRepository::delete);
	}

	@Override
	public boolean existsByName(String nameProduct) {
		return productRepository.existsByName(nameProduct);
	}
	
	@Override
	@Transactional
	@CacheEvict(value = {"products", "productsByCategory"}, allEntries = true)
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

	@Override
	@Cacheable(value = "productsByCategory", key = "'categoryId:' + #categoryId + ':page:' + #pageable.pageNumber + ':limit:' + #pageable.pageSize")
	public CacheablePageResponse<ProductResponse> getProductsByCategoryId(Long categoryId, PageRequest pageable) throws DataNotFoundException {
		Page<ProductResponse> page = productRepository.findByCategoryId(categoryId, pageable)
				.map(ProductResponse::fromEntity);

		return new CacheablePageResponse<>(
				page.getContent(),
				page.getNumber(),
				page.getSize(),
				page.getTotalElements(),
				page.getTotalPages(),
				page.isLast()
		);
	}
}
