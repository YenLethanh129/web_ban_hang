package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Data;
import lombok.experimental.SuperBuilder;

@Data
@SuperBuilder
public class ProfleResponse extends MessageResponse{

    @JsonProperty("user")
    private UserResponse userResponse;
}
