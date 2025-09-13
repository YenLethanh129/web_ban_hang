package com.project.webbanhang.controllers;

import com.project.webbanhang.components.LocalizationUtil;
import com.project.webbanhang.dtos.user.*;
import com.project.webbanhang.models.Customer;
import com.project.webbanhang.models.User;
import com.project.webbanhang.response.*;
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

    /**
     * TOP 10 OWASP 2023
     * API1:2023 - Broken Object Level Authorization (BOLA)
     * Hacker có thể lấy id của người dùng khác và truy cập vào thông tin cá nhân của họ
     * Giải pháp: Sử dụng token để xác thực người dùng hiện tại và chỉ trả về thông tin của họ
     * */
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

    @PostMapping("/forgot-password")
    public ResponseEntity<?> forgotPassword(
    		@Valid @RequestBody ForgotPasswordDTO forgotPasswordDTO,
            BindingResult result
    ) {
        try {
            if (result.hasErrors()) {
                List<String> errorMessages = result.getFieldErrors()
                        .stream()
                        .map(FieldError::getDefaultMessage)
                        .toList();
                return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.FORGOT_PASSWORD_FAILED, errorMessages));
            }

            boolean existsByPhoneNumber = userService.findByPhoneNumber(forgotPasswordDTO.getPhoneNumber());
            if (!existsByPhoneNumber) {
                return ResponseEntity.badRequest().body(
                        MessageResponse.builder()
                                .message(localizationUtil.getLocalizedMessage(MessageKey.FORGOT_PASSWORD_FAILED, "User not found with the provided phone number"))
                                .build()
                );
            }
            return ResponseEntity.ok(
                    MessageResponse.builder()
                            .message("OTP has been sent to your phone number")
                            .build()
            );
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(
                    MessageResponse.builder()
                            .message(localizationUtil.getLocalizedMessage(MessageKey.FORGOT_PASSWORD_FAILED, e.getMessage()))
                            .build()
            );
        }
    }

    @PostMapping("verify-otp")
    public ResponseEntity<?> verifyOtp(
    		@Valid @RequestBody ForgotPasswordDTO forgotPasswordDTO,
            BindingResult result
    ) {
        try {
            if (result.hasErrors()) {
                List<String> errorMessages = result.getFieldErrors()
                        .stream()
                        .map(FieldError::getDefaultMessage)
                        .toList();
                return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.FORGOT_PASSWORD_FAILED, errorMessages));
            }

            String newPassword = userService.forgotPassword(forgotPasswordDTO.getPhoneNumber(), forgotPasswordDTO.getOtpCode());
            return ResponseEntity.ok(
                    ForgotPasswordResponse.builder()
                            .message(localizationUtil.getLocalizedMessage(MessageKey.FORGOT_PASSWORD_SUCCESSFULLY))
                            .newPassword(newPassword)
                            .build()
            );
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(
                    MessageResponse.builder()
                            .message(localizationUtil.getLocalizedMessage(MessageKey.FORGOT_PASSWORD_FAILED, e.getMessage()))
                            .build()
            );
        }
    }
}
