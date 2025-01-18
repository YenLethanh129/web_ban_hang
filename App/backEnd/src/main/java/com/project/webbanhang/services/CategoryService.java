package com.project.webbanhang.services;

import java.util.List;

import lombok.*;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.CategoryDTO;
import com.project.webbanhang.models.Category;
import com.project.webbanhang.repositories.CategoryRepository;

import jakarta.transaction.Transactional;

@Getter
@Data
@Service
@RequiredArgsConstructor
public class CategoryService implements ICategoryService {
	
	private final CategoryRepository categoryRepository;
	
	@Override
	public Category createCategory(CategoryDTO categoryDTO) {
		Category newCategory = Category.builder()
				.name(categoryDTO.getName())
				.build();
		return categoryRepository.save(newCategory);
	}

	@Override
	public Category getCategoryById(Long id) {
		return categoryRepository.findById(id).orElseThrow(() -> new RuntimeException("Category not found!"));
	}

	@Override
	public List<Category> getAllCategories() {
		return categoryRepository.findAll();
	}


	@Override
	@Transactional
	public Category updateCategory(Long categoryId, CategoryDTO categoryDTO) {
		Category existingCategory = getCategoryById(categoryId);
		existingCategory.setName(categoryDTO.getName());
		categoryRepository.save(existingCategory);
		return existingCategory;
	}
	
	@Override
	public void deleteCategory(Long id) {
		categoryRepository.deleteById(id);
	}
	
}
