package com.project.webbanhang.controllers;

import com.project.webbanhang.components.LocalizationUtil;
import com.project.webbanhang.dtos.UserDTO;
import com.project.webbanhang.dtos.UserLoginDTO;
import com.project.webbanhang.models.User;
import com.project.webbanhang.response.LoginResponse;
import com.project.webbanhang.response.RegisterResponse;
import com.project.webbanhang.response.UserResponse;
import com.project.webbanhang.services.IUserService;
import com.project.webbanhang.utils.MessageKey;

import ch.qos.logback.core.subst.Token;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;

import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("${api.prefix}/users")
@RequiredArgsConstructor
public class UserController {
	
	private final IUserService userService;
	private final LocalizationUtil localizationUtil;
	
    @PostMapping("/login")
    public ResponseEntity<LoginResponse> login(
            @Valid @RequestBody UserLoginDTO userLoginDTO
    ) {
        try {
            String token = userService.login(userLoginDTO.getPhoneNumber(), userLoginDTO.getPassword());
            return ResponseEntity.ok(
            		LoginResponse.builder()
            			.message(localizationUtil.getLocalizedMessage(MessageKey.LOGIN_SUCCESSFULLY))
            			.token(token)
            			.build()
            		);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(
            		LoginResponse.builder()
            			.message(localizationUtil.getLocalizedMessage(MessageKey.LOGIN_FAILED, e.getMessage()))
            			.build()
            		);
        }
    }   

    @PostMapping("/register")
    public ResponseEntity<?> createUser(
            @Valid @RequestBody UserDTO userDTO,
            BindingResult result
    ) {
        try {
            if (result.hasErrors()) {
                List<String> errorMessages = result.getFieldErrors()
                        .stream()
                        .map(FieldError::getDefaultMessage)
                        .toList();
                return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.REGISTER_FAILED, errorMessages));
            }
            if (!userDTO.getPassword().equals(userDTO.getRetypePassword())) {
                return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.WRONG_RETYPE_PASSWORD));
            }
            
            User user = userService.createUser(userDTO);
            
            return ResponseEntity.ok(
            			RegisterResponse.builder()
            				.message(localizationUtil.getLocalizedMessage(MessageKey.REGISTER_SUCCESSFULLY))
            				.user(user)
            				.build()
            		);
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.REGISTER_FAILED, e.getMessage()));
        }
    }
    
    @PostMapping("/profile")
    public ResponseEntity<?> getUserProfile(
    		@RequestHeader("Authorization") String token
    ) {
    	try {
    		String extractedToken = token.substring(7); // Loại bỏ "Bearer " từ chuỗi token
    		User user = userService.getUserProfileFromToken(extractedToken);
    		return ResponseEntity.ok(UserResponse.fromEntity(user));
    	} catch (Exception e) {
    		return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.PROFILE_FAILED, e.getMessage()));
    	}
	}
}
