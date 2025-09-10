package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Data;
import lombok.experimental.SuperBuilder;

@Data
@SuperBuilder
public class ForgotPasswordResponse extends MessageResponse{
    @JsonProperty("new_password")
    private String newPassword;
}
