package com.project.webbanhang.services;

import java.util.Optional;

import com.project.webbanhang.components.LocalizationUtil;
import com.project.webbanhang.repositories.CustomerRepository;
import com.project.webbanhang.utils.MessageKey;
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
	private final CustomerRepository customerRepository;
	private final RoleRepository roleRepository;
	private final PasswordEncoder passwordEncoder;
	private final JwtTokenUtil jwtTokenUtil;
	private final AuthenticationManager authenticationManager;
	private final LocalizationUtil localizationUtil;

	@Override
	public User createUser(UserDTO userDTO) throws Exception {
		String phoneNumber = userDTO.getPhoneNumber();
		if (userRepository.existsByPhoneNumber(phoneNumber)) {
			throw new Exception(localizationUtil.getLocalizedMessage(MessageKey.PHONE_HAS_EXIST));
		}

		Optional<Role> existingRole = roleRepository.findByName(userDTO.getRoleName());
		if (existingRole.isEmpty()) {
//			throw new Exception("Server is under maintenance, please try again later!");
			throw new Exception(localizationUtil.getLocalizedMessage(MessageKey.ROLE_NOT_EXIST));
		}
		if (!existingRole.get().getName().toUpperCase().equals(Role.CUSTOMER)) {
//			throw new Exception("You can't register order role!");
			throw new Exception(localizationUtil.getLocalizedMessage(MessageKey.ROLE_NOT_CUSTOMER));
		}

		User newUser = User.builder()
				.fullName(userDTO.getFullName())
				.phoneNumber(userDTO.getPhoneNumber())
				.password(userDTO.getPassword())
				.address(userDTO.getAddress())
				.dateOfBirth(userDTO.getDateOfBirth())
				.facebookAccountId(userDTO.getFacebookAccountId())
				.googleAccountId(userDTO.getGoogleAccountId())
				.isActive(true)
				.build();

		newUser.setRole(existingRole.get());
		
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
