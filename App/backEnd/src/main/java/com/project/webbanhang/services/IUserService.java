package com.project.webbanhang.services;

import com.project.webbanhang.dtos.UserDTO;
import com.project.webbanhang.exceptions.DataNotFoundException;
import com.project.webbanhang.models.User;

public interface IUserService {

	User createUser(UserDTO userDTO) throws DataNotFoundException, Exception;
	
	String login(String phoneNumber, String password) throws Exception;
}
