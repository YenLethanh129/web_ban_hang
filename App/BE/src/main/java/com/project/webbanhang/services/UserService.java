package com.project.webbanhang.services;

import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import com.project.webbanhang.dtos.UserDTO;
import com.project.webbanhang.dtos.UserLoginDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.Role;
import com.project.webbanhang.models.User;
import com.project.webbanhang.repositories.RoleRepository;
import com.project.webbanhang.repositories.UserRepository;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class UserService implements IUserService{

	private UserRepository userRepository;
	private RoleRepository roleRepository;
	
	@Override
	public User createUser(UserDTO userDTO) throws DataNotFoundException {
		String phoneNumber = userDTO.getPhoneNumber();
		if (userRepository.existsByPhoneNumber(phoneNumber)) {
			throw new DataIntegrityViolationException("Phone number already exists");
		}
		// Convert UserDTO -> User
		User newUser = User.builder()
				.fullName(userDTO.getFullName())
				.phoneNumber(userDTO.getPhoneNumber())
				.password(userDTO.getPassword())
				.address(userDTO.getAddress())
				.dateOfBirth(userDTO.getDateOfBirth())
				.facebookAccountId(userDTO.getFacebookAccountId())
				.googleAccountId(userDTO.getGoogleAccountId())
				.build();
		Role role = roleRepository.findById(userDTO.getRoleId())
				.orElseThrow(() -> new DataNotFoundException("Role not found"));
		newUser.setRole(role);
		
		if (userDTO.getFacebookAccountId() == 0 && userDTO.getGoogleAccountId() == 0) {
			String password = userDTO.getPassword();
			//String encodePassword = passwordEncoder.encode(password);
			//newUser.setPassword(encodePassword);
		}
		return userRepository.save(newUser);
	}
	

	@Override
	public String login(String phoneNumber, String password) {
		
		return null;
	}

}
