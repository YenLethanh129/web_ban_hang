package com.project.webbanhang.controllers;

import com.project.webbanhang.components.LocalizationUtil;
import com.project.webbanhang.dtos.user.ChangPasswordDTO;
import com.project.webbanhang.dtos.user.UserDTO;
import com.project.webbanhang.dtos.user.UserLoginDTO;
import com.project.webbanhang.dtos.user.UserUpdateDTO;
import com.project.webbanhang.models.Customer;
import com.project.webbanhang.models.User;
import com.project.webbanhang.response.LoginResponse;
import com.project.webbanhang.response.MessageResponse;
import com.project.webbanhang.response.RegisterResponse;
import com.project.webbanhang.response.UserResponse;
import com.project.webbanhang.services.Interfaces.ICustomerService;
import com.project.webbanhang.services.Interfaces.IUserService;
import com.project.webbanhang.utils.MessageKey;

import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;

import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("${api.prefix}/users")
@RequiredArgsConstructor
public class UserController {
	
	private final IUserService userService;
    private final ICustomerService customerService;
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
            
            User user = userService.createUser(userDTO);

            Customer customer = Customer.builder()
                    .id(user.getId())
                    .user(user)
                    .email(null)
                    .address(user.getAddress())
                    .phoneNumber(user.getPhoneNumber())
                    .fullName(user.getFullName())
                    .build();

            customerService.createCustomer(customer);

            UserResponse userResponse = UserResponse.builder()
                    .userId(user.getId())
                    .phoneNumber(user.getPhoneNumber())
                    .fullName(user.getFullName())
                    .address(user.getAddress())
                    .build();

            return ResponseEntity.ok(
            			RegisterResponse.builder()
            				.message(localizationUtil.getLocalizedMessage(MessageKey.REGISTER_SUCCESSFULLY))
            				.userResponse(userResponse)
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

    @PatchMapping("/update")
    public ResponseEntity<?> updateUserProfile(
    		@RequestHeader("Authorization") String token,
    		@Valid @RequestBody UserUpdateDTO userUpdateDTODTO,
            BindingResult result
    ) {
        try {
            if (result.hasErrors()) {
                List<String> errorMessages = result.getFieldErrors()
                        .stream()
                        .map(FieldError::getDefaultMessage)
                        .toList();
                return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.UPDATE_PROFILE_FAILED, errorMessages));
            }

            String extractedToken = token.substring(7); // Loại bỏ "Bearer " từ chuỗi token
            User updatedUser = userService.updateUserFromToken(extractedToken, userUpdateDTODTO);
            return ResponseEntity.ok(
                    UserResponse.fromEntity(updatedUser)
            );
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.UPDATE_PROFILE_FAILED, e.getMessage()));
        }
    }

    @PostMapping("/update-password")
    public ResponseEntity<?> updatePassword(
    		@RequestHeader("Authorization") String token,
    		@Valid @RequestBody ChangPasswordDTO changPasswordDTO,
            BindingResult result
    ) {
        try {
            if (result.hasErrors()) {
                List<String> errorMessages = result.getFieldErrors()
                        .stream()
                        .map(FieldError::getDefaultMessage)
                        .toList();
                return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.UPDATE_PROFILE_FAILED, errorMessages));
            }

            String extractedToken = token.substring(7); // Loại bỏ "Bearer " từ chuỗi token
            boolean isUpdated = userService.updatePassword(extractedToken, changPasswordDTO);
            if (!isUpdated) {
                return ResponseEntity.badRequest().body(
                        MessageResponse.builder()
                                .message(localizationUtil.getLocalizedMessage(MessageKey.UPDATE_PROFILE_FAILED, "Update password failed"))
                                .build()
                );
            }
            return ResponseEntity.ok(
                    MessageResponse.builder()
                            .message(localizationUtil.getLocalizedMessage(MessageKey.UPDATE_PASSWORD_SUCCESSFULLY))
                            .build()
            );
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(
                    MessageResponse.builder()
                            .message(localizationUtil.getLocalizedMessage(MessageKey.UPDATE_PROFILE_FAILED, e.getMessage()))
                            .build()
            );
        }
    }
}
