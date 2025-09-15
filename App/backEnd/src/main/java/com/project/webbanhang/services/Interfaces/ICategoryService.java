package com.project.webbanhang.services.Interfaces;

import java.util.List;

import com.project.webbanhang.dtos.CategoryDTO;
import com.project.webbanhang.models.Category;

public interface ICategoryService {

	Category createCategory(CategoryDTO categoryDTO);
	
	Category getCategoryById(Long id);
	
	List<Category> getAllCategories();
	
	Category updateCategory(Long categoryId, CategoryDTO categoryDTO);
	
	void deleteCategory(Long id);
}
