package com.project.webbanhang.dtos;

import lombok.*;

@Data
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class GoongPlaceSearchDTO {
    private String input;
    private String location; // lat,lng format: "21.013715,105.798295"
    private Integer limit;
    private Integer radius;
    private Boolean moreCompound;
}
