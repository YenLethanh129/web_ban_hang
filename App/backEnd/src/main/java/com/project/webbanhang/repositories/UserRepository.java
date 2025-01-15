package com.project.webbanhang.repositories;

import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

import com.project.webbanhang.models.User;

public interface UserRepository extends JpaRepository<User, Long>{
	
	boolean existsByPhoneNumber(String phoneNumber);
	
	Optional<User> findByPhoneNumber(String phoneNumber);
}
