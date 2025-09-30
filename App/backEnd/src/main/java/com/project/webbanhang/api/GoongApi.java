package com.project.webbanhang.api;

import com.project.webbanhang.response.GoongPlaceResponse;
import com.project.webbanhang.response.PlaceDetailResponse;
import org.springframework.cloud.openfeign.FeignClient;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;

@FeignClient(name = "goong", url = "${goong.endpoint}")
public interface GoongApi {

    @GetMapping("/Place/AutoComplete")
    GoongPlaceResponse searchPlaces(
            @RequestParam("input") String input,
            @RequestParam("api_key") String apiKey,
            @RequestParam(value = "location", required = false) String location,
            @RequestParam(value = "limit", defaultValue = "10") Integer limit,
            @RequestParam(value = "radius", defaultValue = "50") Integer radius,
            @RequestParam(value = "more_compound", defaultValue = "false") Boolean moreCompound
    );

    @GetMapping("/Place/Detail")
    PlaceDetailResponse getPlaceDetails(
            @RequestParam("place_id") String placeId,
            @RequestParam("api_key") String apiKey
    );}
