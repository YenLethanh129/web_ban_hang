package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.ProductDTO;
import com.project.webbanhang.dtos.ProductImageDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.exceptions.InvalidParamException;
import com.project.webbanhang.models.Product;
import com.project.webbanhang.models.ProductImage;
import com.project.webbanhang.response.ProductResponse;
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

    @GetMapping("")
    public ResponseEntity<?> getProducts(
            @RequestParam(value = "page", defaultValue = "1") int page,
            @RequestParam(value = "limit", defaultValue = "10") int limit
    ) {
    	try {
    		PageRequest pageRequest = PageRequest.of(page, limit, Sort.by("createdAt").descending());
        	Page<ProductResponse> productPage = productService.getAllProducts(pageRequest);
        	List<ProductResponse> products = productPage.getContent();
        	
        	int totalPages = productPage.getTotalPages();
        	
            return ResponseEntity.ok(products);
		} catch (Exception e) {
			return ResponseEntity.badRequest().body(e.getMessage());
		}
    }

    @GetMapping("/{id}")
    public ResponseEntity<String> getProductById(@PathVariable("id") Long productId) {
        return ResponseEntity.ok("Get product with id: " + productId);
    }

    //@PostMapping(value = "", consumes = MediaType.MULTIPART_FORM_DATA_VALUE) - Done
    @PostMapping("")
    public ResponseEntity<?> createProduct(
            @Valid @RequestBody ProductDTO productDTO,
            //@RequestPart("file") MultipartFile file,
            BindingResult result
    ) {
        try {
            // Kiểm tra lỗi
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

    @PutMapping("/{id}")
    public ResponseEntity<String> updateProduct(@PathVariable("id") Long id) {
        return ResponseEntity.ok("Update product with id: " + id);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<String> deleteProduct(@PathVariable("id") Long id) {
        return ResponseEntity.ok("Delete product with id: " + id);
    }

    // Lưu ảnh vào thư mục - Done
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
}
