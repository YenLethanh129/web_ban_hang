package com.project.webbanhang.services;

import java.util.Optional;

import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import com.project.webbanhang.components.JwtTokenUtil;
import com.project.webbanhang.dtos.UserDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.Role;
import com.project.webbanhang.models.User;
import com.project.webbanhang.repositories.RoleRepository;
import com.project.webbanhang.repositories.UserRepository;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class UserService implements IUserService{

	private final UserRepository userRepository;
	private final RoleRepository roleRepository;
	private final PasswordEncoder passwordEncoder;
	private final JwtTokenUtil jwtTokenUtil;
	private final AuthenticationManager authenticationManager;
	
	@Override
	public User createUser(UserDTO userDTO) throws Exception {
		String phoneNumber = userDTO.getPhoneNumber();
		if (userRepository.existsByPhoneNumber(phoneNumber)) {
			throw new DataIntegrityViolationException("Phone number already exists");
		}
		Role role = roleRepository.findById(userDTO.getRoleId())
				.orElseThrow(() -> new DataNotFoundException("Role not found"));
		if (role.getName().toUpperCase().equals(Role.ADMIN)) {
			throw new Exception("You cann't register an admin account");
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

		newUser.setRole(role);
		
		if (userDTO.getFacebookAccountId() == 0 && userDTO.getGoogleAccountId() == 0) {
			String password = userDTO.getPassword();
			String encodePassword = passwordEncoder.encode(password);
			newUser.setPassword(encodePassword);
		}
		return userRepository.save(newUser);
	}
	

	@Override
	public String login(String phoneNumber, String password) throws Exception {
		Optional<User> optionalUser = userRepository.findByPhoneNumber(phoneNumber);
		if (optionalUser.isEmpty()) {
			throw new DataNotFoundException("Invalid phonenumber or password");
		}
		
		User existingUser = optionalUser.get();
		if (existingUser.getGoogleAccountId() == 0 && existingUser.getFacebookAccountId() == 0) {
			if (!passwordEncoder.matches(password, existingUser.getPassword())) {
				throw new BadCredentialsException("Wrong phone number or password");
			}
		}
		
		UsernamePasswordAuthenticationToken authenticationToken = new UsernamePasswordAuthenticationToken(phoneNumber, password, existingUser.getAuthorities());
		
		authenticationManager.authenticate(authenticationToken);
		String token = jwtTokenUtil.generateToken(existingUser);
		return token;
	}


	@Override
	public User getUserProfileFromToken(String extractedToken) throws Exception {
		if (jwtTokenUtil.isTokenExpired(extractedToken)) {
			throw new Exception("Token is expired");
		}
		String phoneNumber = jwtTokenUtil.extractPhoneNumber(extractedToken);
		if (phoneNumber == null) {
			throw new Exception("Invalid Token");
		}
		
		Optional<User> optionalUser = userRepository.findByPhoneNumber(phoneNumber);
		if (optionalUser.isEmpty()) {
			throw new Exception("User not found by phone number");
		}
		return optionalUser.get();
	}

}
