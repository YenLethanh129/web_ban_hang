package com.project.webbanhang.controllers;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.project.webbanhang.components.LocalizationUtil;
import com.project.webbanhang.dtos.UserLoginDTO;
import com.project.webbanhang.services.IUserService;
import com.project.webbanhang.utils.MessageKey;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;
import org.springframework.test.web.servlet.result.MockMvcResultMatchers;

@SpringBootTest
@AutoConfigureMockMvc(addFilters = false)
//@WebMvcTest(UserController.class)
public class UserControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private IUserService iUserService;

    @MockBean
    private LocalizationUtil localizationUtil;

    private UserLoginDTO userLoginDTO;
    private final ObjectMapper objectMapper = new ObjectMapper();

    // khởi tạo các biến trước khi chạy test
    @BeforeEach
    void initData(){
        userLoginDTO = UserLoginDTO.builder()
                .phoneNumber("0123456789")
                .password("wrongpassword")
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
        String errorMessage = "Đăng nhập thất bại: Sai thông tin";;

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
}
