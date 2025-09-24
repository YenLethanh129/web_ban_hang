package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;

@Builder
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class ReceiverResponse {
    private String fullname;

    @JsonProperty("phone_number")
    private String phoneNumber;

    private String address;
}
