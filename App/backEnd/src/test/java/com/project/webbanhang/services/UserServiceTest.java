package com.project.webbanhang.services;

import com.project.webbanhang.components.JwtTokenUtil;
import com.project.webbanhang.dtos.UserDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.Role;
import com.project.webbanhang.models.User;
import com.project.webbanhang.repositories.RoleRepository;
import com.project.webbanhang.repositories.UserRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.test.web.servlet.MockMvc;

import java.sql.Date;
import java.time.LocalDate;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
@AutoConfigureMockMvc(addFilters = false)
public class UserServiceTest {
    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private UserService userService;

    @MockBean
    private UserRepository userRepository;

    @MockBean
    private RoleRepository roleRepository;

    @MockBean
    private PasswordEncoder passwordEncoder;

    @MockBean
    private JwtTokenUtil jwtTokenUtil;

    @MockBean
    private AuthenticationManager authenticationManager;

    private UserDTO userDTO;
    private Role role;
    private User user;

    @BeforeEach
    void initData() {
        userDTO = UserDTO.builder()
                .fullName("Test User")
                .phoneNumber("0123456789")
                .address("TP Ho Chi Minh")
                .password("password")
                .retypePassword("password")
                .dateOfBirth(Date.valueOf(LocalDate.of(2004, 10, 10)))
                .facebookAccountId(Long.valueOf(0))
                .googleAccountId(Long.valueOf(0))
                .roleId(1L)
                .build();

        role = Role.builder()
                .id(1L)
                .name("USER")
                .build();

//        user = User.builder()
//                .fullName("Test Register")
//                .phoneNumber("0123456789")
//                .address("TP Ho Chi Minh")
//                .password("password")
//                .dateOfBirth(Date.valueOf(LocalDate.of(2004, 10, 10)))
//                .facebookAccountId(Long.valueOf(0))
//                .googleAccountId(Long.valueOf(0))
//                .role(role)
//                .build();
    }

    @Test
    void createUser_success() throws Exception {
        Mockito.when(userRepository.existsByPhoneNumber(userDTO.getPhoneNumber()))
                .thenReturn(false);

        Mockito.when(roleRepository.findById(userDTO.getRoleId()))
                .thenReturn(Optional.of(role));

        User newUser = User.builder()
                .fullName(userDTO.getFullName())
                .phoneNumber(userDTO.getPhoneNumber())
                .password(userDTO.getPassword())
                .address(userDTO.getAddress())
                .dateOfBirth(userDTO.getDateOfBirth())
                .facebookAccountId(userDTO.getFacebookAccountId())
                .googleAccountId(userDTO.getGoogleAccountId())
                .build();

        newUser.setPassword("encodePassword");
        newUser.setRole(role);

        Mockito.when(userRepository.save(Mockito.any(User.class)))
                .thenReturn(newUser);

        User result = userService.createUser(userDTO);

        assertNotNull(result);
        assertEquals("Test User", result.getFullName());
        assertEquals("encodePassword", result.getPassword());
        assertEquals(role, result.getRole());
    }

    @Test
    void createUser_fail_existsByPhoneNumber() throws Exception {
        Mockito.when(userRepository.existsByPhoneNumber(userDTO.getPhoneNumber()))
                .thenReturn(true);

        Exception exception = assertThrows(DataIntegrityViolationException.class, () -> {
            userService.createUser(userDTO);
        });

        assertEquals("Phone number already exists", exception.getMessage());
    }

    @Test
    void createUser_fail_roleNotFound() throws Exception {
        Mockito.when(userRepository.existsByPhoneNumber(userDTO.getPhoneNumber()))
                .thenReturn(false);

        Mockito.when(roleRepository.findById(userDTO.getRoleId()))
                .thenReturn(Optional.empty());

        Exception exception = assertThrows(DataNotFoundException.class, () -> {
            userService.createUser(userDTO);
        });

        assertEquals("Role not found", exception.getMessage());
    }

    @Test
    void createUser_fail_registerAdmin() throws Exception{
        Role roleAdmin = Role.builder()
                .id(2L)
                .name("ADMIN")
                .build();

        Mockito.when(userRepository.existsByPhoneNumber(userDTO.getPhoneNumber()))
                .thenReturn(false);

        Mockito.when(roleRepository.findById(userDTO.getRoleId()))
                .thenReturn(Optional.of(roleAdmin));

        Exception exception = assertThrows(Exception.class, () -> {
            userService.createUser(userDTO);
        });

        assertEquals("You cann't register an admin account", exception.getMessage());
    }
}
