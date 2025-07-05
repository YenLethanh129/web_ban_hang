package com.project.webbanhang.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.project.webbanhang.components.LocalizationUtil;
import com.project.webbanhang.dtos.UserDTO;
import com.project.webbanhang.dtos.UserLoginDTO;
import com.project.webbanhang.models.Role;
import com.project.webbanhang.models.User;
import com.project.webbanhang.response.UserResponse;
import com.project.webbanhang.services.IUserService;
import com.project.webbanhang.utils.MessageKey;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;
import org.springframework.test.web.servlet.result.MockMvcResultMatchers;

import java.sql.Date;
import java.time.LocalDate;
import java.util.Map;

@SpringBootTest
@AutoConfigureMockMvc(addFilters = false)
public class UserControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private IUserService iUserService;

    @MockBean
    private LocalizationUtil localizationUtil;

    private UserLoginDTO userLoginDTO;
    private UserDTO userDTO;
    private User user;
    private Role role;
    private final ObjectMapper objectMapper = new ObjectMapper();

    // khởi tạo các biến trước khi chạy test
    @BeforeEach
    void initData(){
        objectMapper.configure(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS, false);

        userLoginDTO = UserLoginDTO.builder()
                .phoneNumber("0123456789")
                .password("password")
                .build();

        userDTO = UserDTO.builder()
                .fullName("Test Register")
                .phoneNumber("0123456789")
                .address("TP Ho Chi Minh")
                .password("password")
                .retypePassword("password")
                .dateOfBirth(Date.valueOf(LocalDate.of(2004, 10, 10)))
                .facebookAccountId(Long.valueOf(0))
                .googleAccountId(Long.valueOf(0))
                .roleId(Long.valueOf(1))
                .build();

        role = Role.builder()
                .id(1L)
                .name("USER")
                .build();

        user = User.builder()
                .fullName("Test Register")
                .phoneNumber("0123456789")
                .address("TP Ho Chi Minh")
                .password("password")
                .dateOfBirth(Date.valueOf(LocalDate.of(2004, 10, 10)))
                .facebookAccountId(Long.valueOf(0))
                .googleAccountId(Long.valueOf(0))
                .role(role)
                .build();
    }

    @Test
    void login_validRequest_success() throws Exception {
        // GIVEN
        String token = "mockedToken";
        String successMessage = "Đăng nhập thành công";

        Mockito.when(iUserService.login(Mockito.anyString(), Mockito.anyString()))
                .thenReturn(token);
        Mockito.when(localizationUtil.getLocalizedMessage(MessageKey.LOGIN_SUCCESSFULLY))
                .thenReturn(successMessage);

        // WHEN & THEN
        mockMvc.perform(MockMvcRequestBuilders.post("/api/v1/users/login")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(userLoginDTO)))
                .andExpect(MockMvcResultMatchers.status().isOk())
                .andExpect(MockMvcResultMatchers.jsonPath("$.message").value(successMessage))
                .andExpect(MockMvcResultMatchers.jsonPath("$.token").value(token));
    }

    @Test
    void login_invalidRequest_fail() throws Exception {
        // GIVEN
        String errorMessage = "Đăng nhập thất bại: Sai thông tin";

        Mockito.when(iUserService.login(Mockito.anyString(), Mockito.anyString()))
                .thenThrow(new RuntimeException("Sai thông tin"));

        Mockito.when(localizationUtil.getLocalizedMessage(MessageKey.LOGIN_FAILED, "Sai thông tin"))
                .thenReturn(errorMessage);

        // WHEN & THEN
        mockMvc.perform(MockMvcRequestBuilders.post("/api/v1/users/login")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(userLoginDTO)))
                .andExpect(MockMvcResultMatchers.status().isBadRequest())
                .andExpect(MockMvcResultMatchers.jsonPath("$.message").value(errorMessage))
                .andExpect(MockMvcResultMatchers.jsonPath("$.token").doesNotExist());
    }

    @Test
    void register_validRequest_success() throws Exception {
        String registerSuccess = "Đăng kí tài khoản người dùng thành công!";
        String resulJson = objectMapper.writeValueAsString(
                Map.of(
                        "message", registerSuccess,
                        "user", user
                )
        );

        Mockito.when(iUserService.createUser(Mockito.any()))
                .thenReturn(user);

        Mockito.when(localizationUtil.getLocalizedMessage(MessageKey.REGISTER_SUCCESSFULLY))
                .thenReturn(registerSuccess);

        mockMvc.perform(MockMvcRequestBuilders.post("/api/v1/users/register")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(userDTO)))
                .andExpect(MockMvcResultMatchers.status().isOk())
                .andExpect(MockMvcResultMatchers.content().json(resulJson));
    }

    @Test
    void register_invalidRequest_fail() throws Exception {
        String errorMessage = "Đăng kí tài khoản người dùng không thành công!";

        Mockito.when(iUserService.createUser(Mockito.any()))
                .thenThrow(new RuntimeException("Đã có lỗi xảy ra khi kí thông tin người dùng!"));

        Mockito.when(localizationUtil.getLocalizedMessage(MessageKey.REGISTER_FAILED, "Đã có lỗi xảy ra khi kí thông tin người dùng!"))
                .thenReturn(errorMessage);

        mockMvc.perform(MockMvcRequestBuilders.post("/api/v1/users/register")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(userDTO)))
                .andExpect(MockMvcResultMatchers.status().isBadRequest())
                .andExpect(MockMvcResultMatchers.content().string(errorMessage));
    }

    @Test
    void getUserProfile_success() throws Exception {
        String token = "Bearer mockToken";
        String extractedToken = "mockToken";
        String resulJson = objectMapper.writeValueAsString(UserResponse.fromEntity(user));

        Mockito.when(iUserService.getUserProfileFromToken(extractedToken))
                .thenReturn(user);

        mockMvc.perform(MockMvcRequestBuilders.post("/api/v1/users/profile")
                        .header("Authorization", token)
                        .contentType(MediaType.APPLICATION_JSON))
                .andExpect(MockMvcResultMatchers.status().isOk())
                .andExpect(MockMvcResultMatchers.content().json(resulJson));
    }

    @Test
    void getUserProfile_fail() throws Exception {
        String token = "Bearer mockToken";
        String extractedToken = "mockToken";
        String errorMessage = "Lỗi khi lấy thông tin người dùng!";
        String resulJson = objectMapper.writeValueAsString(UserResponse.fromEntity(user));

        Mockito.when(iUserService.getUserProfileFromToken(extractedToken))
                .thenThrow(new RuntimeException("Token đã hết hạn!"));

        Mockito.when(localizationUtil.getLocalizedMessage(MessageKey.PROFILE_FAILED, "Token đã hết hạn!"))
                .thenReturn(errorMessage);

        mockMvc.perform(MockMvcRequestBuilders.post("/api/v1/users/profile")
                        .header("Authorization", token)
                        .contentType(MediaType.APPLICATION_JSON))
                .andExpect(MockMvcResultMatchers.status().isBadRequest())
                .andExpect(MockMvcResultMatchers.content().string(errorMessage));
    }
}
