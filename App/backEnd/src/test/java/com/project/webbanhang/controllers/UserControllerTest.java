package com.project.webbanhang.controllers;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.project.webbanhang.dtos.UserLoginDTO;
import com.project.webbanhang.response.LoginResponse;
import com.project.webbanhang.services.IUserService;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.ArgumentMatchers;
import org.mockito.Mockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;
import org.springframework.test.web.servlet.result.MockMvcResultMatchers;

@SpringBootTest
@AutoConfigureMockMvc(addFilters = false)
public class UserControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockBean
    private IUserService iUserService;

    private UserLoginDTO userLoginDTO;
    private LoginResponse loginResponse;

    // khởi tạo các biến trước khi chạy test
    @BeforeEach
    void initData(){
        userLoginDTO = UserLoginDTO.builder()
                .phoneNumber("0375440580")
                .password("thanhyen")
                .build();

        loginResponse = LoginResponse.builder()
                .message("Login success!")
                .token("dalshdaksjhd123hjs623")
                .build();
    }

    @Test
    void login_validRequest_success() throws Exception{
        // GIVEN (TAO DATA)
        ObjectMapper objectMapper = new ObjectMapper();
        String content = "";
        try {
            content = objectMapper.writeValueAsString(userLoginDTO);
        } catch (JsonProcessingException e) {
            e.printStackTrace();
        }

        Mockito.when(iUserService.login(ArgumentMatchers.any(), ArgumentMatchers.any()))
                .thenReturn("dalshdaksjhd123hjs623");

        // WHEN
        mockMvc.perform(MockMvcRequestBuilders
                .post("/api/v1/users/login")
                .contentType(MediaType.APPLICATION_JSON_VALUE)
                .content(content))
            .andExpect(MockMvcResultMatchers.status().isOk());
    }
}
