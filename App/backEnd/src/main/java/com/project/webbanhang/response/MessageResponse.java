package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Builder;
import lombok.Data;
import lombok.experimental.SuperBuilder;

@Data
@SuperBuilder
public class MessageResponse {
    @JsonProperty("message")
    private String message;
}
