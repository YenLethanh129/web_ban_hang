package com.project.webbanhang.controllers;

import com.project.webbanhang.components.LocalizationUtil;
import com.project.webbanhang.dtos.user.*;
import com.project.webbanhang.models.Customer;
import com.project.webbanhang.models.User;
import com.project.webbanhang.response.*;
import com.project.webbanhang.services.Interfaces.ICustomerService;
import com.project.webbanhang.services.Interfaces.IUserService;
import com.project.webbanhang.utils.MessageKey;

import jakarta.servlet.http.Cookie;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;

import org.springframework.http.HttpHeaders;
import org.springframework.http.ResponseCookie;
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

    /**
     * TOP 10 OWASP 2023
     * API2: 2023 - Cryptographic Failures
     * Giải pháp: Sử dụng JWT để làm token và lưu trữ trong HttpOnly Cookie
     * */
    @PostMapping("/login")
    public ResponseEntity<?> login(
            @Valid @RequestBody UserLoginDTO userLoginDTO
    ) {
        try {
            String token = userService.login(userLoginDTO.getPhoneNumber(), userLoginDTO.getPassword());
            ResponseCookie cookie = ResponseCookie.from("JWT_TOKEN", token)
                    .httpOnly(true) // Chỉ cho phép truy cập cookie từ phía server
                    .secure(false) // Chỉ gửi cookie qua kết nối HTTPS
                    .path("/") // Cookie sẽ có hiệu lực trên toàn bộ ứng dụng
                    .sameSite("Lax") // Ngăn chặn việc gửi cookie trong các yêu cầu bên thứ ba
                    .maxAge(24 * 60 * 60) // Thời gian sống của cookie là 1 ngày)
                    .build();

            return ResponseEntity.ok()
                    .header(HttpHeaders.SET_COOKIE, cookie.toString())
                    .body(
                        LoginResponse.builder()
                            .message(localizationUtil.getLocalizedMessage(MessageKey.LOGIN_SUCCESSFULLY))
                            .status(200)
                            .build()
                    );
        } catch (Exception e) {
            return ResponseEntity.badRequest().body(
                    LoginResponse.builder()
                            .message(localizationUtil.getLocalizedMessage(MessageKey.LOGIN_FAILED, e.getMessage()))
                            .status(400)
                            .build()
            );
        }
    }

    @PostMapping("/logout")
    public ResponseEntity<?> logout()
    {
        ResponseCookie cookie = ResponseCookie.from("JWT_TOKEN", "")
                .httpOnly(true) // Chỉ cho phép truy cập cookie từ phía server
                .secure(false) // Chỉ gửi cookie qua kết nối HTTPS
                .path("/") // Cookie sẽ có hiệu lực trên toàn bộ ứng dụng
                .sameSite("Strict") // Ngăn chặn việc gửi cookie trong các yêu cầu bên thứ ba
                .maxAge(0) // Xóa cookie ngay lập tức
                .build();

        return ResponseEntity.ok()
                .header(HttpHeaders.SET_COOKIE, cookie.toString())
                .body(
                        MessageResponse.builder()
                                .message(localizationUtil.getLocalizedMessage(MessageKey.LOGOUT_SUCCESSFULLY))
                                .build()
                );
    }

//    @PostMapping("is_login")
//    public ResponseEntity<?> isLogin(

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
    		HttpServletRequest request
    ) {
    	try {
            Cookie[] cookies = request.getCookies();
            String extractedToken = null;
            if (cookies != null) {
                for (Cookie cookie : cookies) {
                    if (cookie.getName().equals("JWT_TOKEN")) {
                        extractedToken = cookie.getValue();
                        break;
                    }
                }
            }
    		User user = userService.getUserProfileFromToken(extractedToken);
    		return ResponseEntity.ok(
                    ProfleResponse.builder()
                        .message(localizationUtil.getLocalizedMessage(MessageKey.PROFILE_SUCCESSFULLY))
                        .userResponse(UserResponse.fromEntity(user))
                        .build()
            );
    	} catch (Exception e) {
    		return ResponseEntity.badRequest().body(localizationUtil.getLocalizedMessage(MessageKey.PROFILE_FAILED, e.getMessage()));
    	}
	}

    @PatchMapping("/update")
    public ResponseEntity<?> updateUserProfile(
    		HttpServletRequest request,
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

            Cookie[] cookies = request.getCookies();
            String extractedToken = null;
            if (cookies != null) {
                for (Cookie cookie : cookies) {
                    if (cookie.getName().equals("JWT_TOKEN")) {
                        extractedToken = cookie.getValue();
                        break;
                    }
                }
            }

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
    		HttpServletRequest request,
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

            Cookie[] cookies = request.getCookies();
            String extractedToken = null;
            if (cookies != null) {
                for (Cookie cookie : cookies) {
                    if (cookie.getName().equals("JWT_TOKEN")) {
                        extractedToken = cookie.getValue();
                        break;
                    }
                }
            }
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
