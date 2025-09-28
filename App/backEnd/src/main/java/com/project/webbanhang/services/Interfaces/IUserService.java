package com.project.webbanhang.services.Interfaces;

import com.project.webbanhang.dtos.user.ChangPasswordDTO;
import com.project.webbanhang.dtos.user.UserDTO;
import com.project.webbanhang.dtos.user.UserUpdateDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.User;

public interface IUserService {

	User createUser(UserDTO userDTO) throws DataNotFoundException, Exception;
	
	String login(String phoneNumber, String password) throws Exception;
	
	User getUserProfileFromToken(String extractedToken) throws Exception;

	User updateUserFromToken(String extractedToken, UserUpdateDTO userUpdateDTO) throws Exception;

	boolean updatePassword(String extractedToken, ChangPasswordDTO changPasswordDTO) throws Exception;

	boolean findByPhoneNumber(String phoneNumber) throws Exception;

	String forgotPassword (String phoneNumber, String otp) throws Exception;
}
