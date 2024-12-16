package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;

public class ProductResponse extends BaseResponse{
    private String name;
    private float price;
    private String thumbnail;
    private String description;

    @JsonProperty("category_id")
    private Long categoryId;
}
