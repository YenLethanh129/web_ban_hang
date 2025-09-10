package com.project.webbanhang.services;

import java.util.Optional;

import com.project.webbanhang.components.LocalizationUtil;
import com.project.webbanhang.dtos.user.ChangPasswordDTO;
import com.project.webbanhang.dtos.user.UserUpdateDTO;
import com.project.webbanhang.services.Interfaces.IUserService;
import com.project.webbanhang.utils.MessageKey;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import com.project.webbanhang.components.JwtTokenUtil;
import com.project.webbanhang.dtos.user.UserDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.Role;
import com.project.webbanhang.models.User;
import com.project.webbanhang.repositories.RoleRepository;
import com.project.webbanhang.repositories.UserRepository;

import lombok.RequiredArgsConstructor;

@Service
@RequiredArgsConstructor
public class UserService implements IUserService {

	private final UserRepository userRepository;
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

		Optional<Role> existingRole = roleRepository.findByName(Role.CUSTOMER);
		if (existingRole.isEmpty()) {
			throw new Exception(localizationUtil.getLocalizedMessage(MessageKey.ROLE_NOT_EXIST));
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

	@Override
	public User updateUserFromToken(String extractedToken, UserUpdateDTO userUpdateDTO) throws Exception {
		if (jwtTokenUtil.isTokenExpired(extractedToken)) {
			throw new Exception("Token is expired");
		}
		String phoneNumber = jwtTokenUtil.extractPhoneNumber(extractedToken);
		if (phoneNumber == null) {
			throw new Exception("Invalid Token");
		}

		Optional<User> optionalUser = userRepository.findByPhoneNumber(phoneNumber);
		if (optionalUser.isEmpty()) {
			throw new DataNotFoundException(localizationUtil.getLocalizedMessage(MessageKey.USER_NOT_FOUND));
		}

		User existingUser = optionalUser.get();
		existingUser.setFullName(userUpdateDTO.getFullName());
		existingUser.setAddress(userUpdateDTO.getAddress());
		existingUser.setDateOfBirth(userUpdateDTO.getDateOfBirth());
//		String password = userUpdateDTO.getPassword();
//		if (password != null && !password.isEmpty()) {
//			String encodePassword = passwordEncoder.encode(password);
//			existingUser.setPassword(encodePassword);
//		}

		try {
			return userRepository.save(existingUser);
		} catch (DataIntegrityViolationException e) {
			throw new Exception(localizationUtil.getLocalizedMessage(MessageKey.UPDATE_PROFILE_FAILED, "Phone number has existed"));
		} catch (Exception e) {
			throw new Exception(localizationUtil.getLocalizedMessage(MessageKey.UPDATE_PROFILE_FAILED, e.getMessage()));
		}
	}

	@Override
	public boolean updatePassword(String extractedToken, ChangPasswordDTO changPasswordDTO) throws Exception {
		if (jwtTokenUtil.isTokenExpired(extractedToken)) {
			throw new Exception("Token is expired");
		}
		String phoneNumber = jwtTokenUtil.extractPhoneNumber(extractedToken);
		if (phoneNumber == null) {
			throw new Exception("Invalid Token");
		}

		Optional<User> optionalUser = userRepository.findByPhoneNumber(phoneNumber);
		if (optionalUser.isEmpty()) {
			throw new DataNotFoundException(localizationUtil.getLocalizedMessage(MessageKey.USER_NOT_FOUND));
		}
		User existingUser = optionalUser.get();
		String oldPassword = changPasswordDTO.getOldPassword();
		if (!passwordEncoder.matches(oldPassword, existingUser.getPassword())) {
			throw new Exception("Mat khau cu khong chinh xac!");
		}
		String newPassword = changPasswordDTO.getNewPassword();
		String encodePassword = passwordEncoder.encode(newPassword);
		existingUser.setPassword(encodePassword);

		try {
			userRepository.save(existingUser);
			return true;
		} catch (Exception e) {
			throw new Exception(localizationUtil.getLocalizedMessage(MessageKey.UPDATE_PROFILE_FAILED, e.getMessage()));
		}
	}

	@Override
	public User findByPhoneNumber(String phoneNumber) throws Exception {
		return null;
	}

	@Override
	public String forgotPassword(String phoneNumber, String otp) throws Exception {
		return "";
	}

}
