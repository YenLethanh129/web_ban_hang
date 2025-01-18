package com.project.webbanhang.controllers;

import com.github.javafaker.Faker;
import com.project.webbanhang.dtos.ProductDTO;
import com.project.webbanhang.dtos.ProductImageDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.exceptions.InvalidParamException;
import com.project.webbanhang.models.Product;
import com.project.webbanhang.models.ProductImage;
import com.project.webbanhang.response.ProductListResponse;
import com.project.webbanhang.response.ProductResponse;
import com.project.webbanhang.services.IProductImageService;
import com.project.webbanhang.services.IProductService;

import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.util.StringUtils;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.StandardCopyOption;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@RestController
@RequiredArgsConstructor
@RequestMapping("${api.prefix}/products")
public class ProductController {
	
	private final IProductService productService;
	//private final IProductImageService productImageService;

	// Done
    @GetMapping("")
    public ResponseEntity<?> getProducts(
            @RequestParam(value = "page", defaultValue = "1") int page,
            @RequestParam(value = "limit", defaultValue = "10") int limit
    ) {
    	try {
    		//PageRequest pageRequest = PageRequest.of(page, limit, Sort.by("createdAt").descending());
    		PageRequest pageRequest = PageRequest.of(page, limit);
        	Page<ProductResponse> productPage = productService.getAllProducts(pageRequest);
        	List<ProductResponse> products = productPage.getContent();
        	
        	int totalPages = productPage.getTotalPages();
        	
        	ProductListResponse productListResponse = ProductListResponse.builder()
        			.products(products)
        			.totalPage(totalPages)
        			.build();
        	
            return ResponseEntity.ok(productListResponse);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body(e.getMessage());
		}
    }

    // Done
    @GetMapping("/{id}")
    public ResponseEntity<?> getProductById(
    		@PathVariable("id") Long productId
    ) {
    	try {
			Product existingProduct = productService.getProductById(productId);
			return ResponseEntity.ok(ProductResponse.fromEntity(existingProduct));
		} catch (DataNotFoundException e) {
			return ResponseEntity.badRequest().body(e.getMessage());
		}
    }

    // Done
    //@PostMapping(value = "", consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    @PostMapping("")
    public ResponseEntity<?> createProduct(
            @Valid @RequestBody ProductDTO productDTO,
            //@RequestPart("file") MultipartFile file,
            BindingResult result
    ) {
        try {
            if (result.hasErrors()) {
                List<String> errorMessages = result.getFieldErrors()
                        .stream()
                        .map(FieldError::getDefaultMessage)
                        .toList();
                return ResponseEntity.badRequest().body(errorMessages);
            }
            
            productService.createProduct(productDTO);

            return ResponseEntity.ok("Insert product successfully");
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }
    
    // Done
    @PostMapping(value = "uploads/{id}",
    		consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public ResponseEntity<?> uploadImages(
    		@PathVariable("id") Long productId,
    		@RequestParam("files") List<MultipartFile> files
    ){
    	try {
    		Product existingProduct = productService.getProductById(productId);
    		if (files == null || files.isEmpty() || (files.size() == 1 && files.get(0).getOriginalFilename().isEmpty())) { 
    			return ResponseEntity.badRequest().body("No image is selected!"); 
    		}
            if (files.size() > 5) { 
            	return ResponseEntity.badRequest().body("You can only upload maximun 5 images!");
            }
            List<ProductImage> productImages = new ArrayList<>();
            for (MultipartFile file : files) {
                if (file.getSize() == 0) {
                    continue;
                }
                if (file.getSize() > 10 * 1024 * 1024) { // 10Mb
                    return ResponseEntity.status(HttpStatus.PAYLOAD_TOO_LARGE).body("File size is too large > 10Mb");
                }
                String contentType = file.getContentType();
                if (contentType != null && !contentType.startsWith("image/")) {
                    return ResponseEntity.status(HttpStatus.UNSUPPORTED_MEDIA_TYPE).body("File type is not supported");
                }
                String fileName = storeFile(file);
                
                ProductImage productImage = productService.createProductImage(
                		existingProduct.getId(),
                		ProductImageDTO.builder()
                			.imageUrl(fileName)
                			.build()
                );
                productImages.add(productImage);
            }
            return ResponseEntity.ok().body(productImages);
    	} catch (DataNotFoundException | InvalidParamException e) {
    		return ResponseEntity.badRequest().body(e.getMessage());
    	} catch (Exception e) {
            return ResponseEntity.badRequest().body(e.getMessage());
        }
    }

    //
    @PutMapping("/{id}")
    public ResponseEntity<?> updateProduct(
    		@PathVariable("id") Long id,
    		@RequestBody ProductDTO productDTO
    ) {
    	try {
			Product updateProduct = productService.updateProduct(id, productDTO);
			return ResponseEntity.ok(ProductResponse.fromEntity(updateProduct));
		} catch (Exception e) {
			return ResponseEntity.status(HttpStatus.BAD_REQUEST).body(e.getMessage());
		}
    }

    // Done
    @DeleteMapping("/{id}")
    public ResponseEntity<String> deleteProduct(
    		@PathVariable("id") Long id
    ) {
    	productService.deleteProduct(id);
        return ResponseEntity.ok("Delete product with id: " + id);
    }

    // Done
    // Lưu ảnh vào thư mục
    private String storeFile(MultipartFile file) throws IOException {
        String fileName = StringUtils.cleanPath(file.getOriginalFilename());
        String uniqueFileName = UUID.randomUUID().toString() + "_" + fileName;
        Path uploadDir = Paths.get("uploads");

        if (!Files.exists(uploadDir)) {
            Files.createDirectories(uploadDir);
        }

        Path destination = Paths.get(uploadDir.toString(), uniqueFileName);
        Files.copy(file.getInputStream(), destination, StandardCopyOption.REPLACE_EXISTING);
        return uniqueFileName;
    }
    
    // Done
    //@PostMapping("/generateFakeProducts")
    /*
    private ResponseEntity<?> generateFakeProducts(){
    	Faker faker = new Faker();
    	for (int i = 0; i < 300; i++) {
        	String title = faker.commerce().productName();
        	if (productService.existsByName(title)) {
        		continue;
        	}
        	
    		ProductDTO productDTO = ProductDTO.builder()
    				.name(title)
    				.price(faker.number().numberBetween(1, 1000000000))
    				.description(faker.lorem().sentence())
    				.thumbnail("")
    				.categoryId((long)faker.number().numberBetween(1, 3))
    				.build();
    		try {
				productService.createProduct(productDTO);
			} catch (DataNotFoundException e) {
				ResponseEntity.badRequest().body(e.getMessage());
			}
    	}
    	return ResponseEntity.ok("Generate fake products");
    }
    */
    
//    @PostMapping("/generateFakeProductImages")
//    public ResponseEntity<?> generateFakeProductImages() throws DataNotFoundException {
//        Faker faker = new Faker();
//
//        for (int i = 0; i < 100; i++) {
//            Long productId = (long) faker.number().numberBetween(1, 20);
//            ProductImageDTO productImageDTO = ProductImageDTO.builder()
//                    .productId(productId)
//                    .imageUrl("https://picsum.photos/seed/picsum/200/300")
//                    .build();
//
//            productImageService.createProductImage(productImageDTO);
//        }
//
//        return ResponseEntity.ok("Generated 100 fake product images.");
//    }
}
