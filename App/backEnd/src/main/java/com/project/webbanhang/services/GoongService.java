package com.project.webbanhang.services;

import com.project.webbanhang.api.GoongApi;
import com.project.webbanhang.dtos.goong.GoongPlaceSearchDTO;
import com.project.webbanhang.response.GoongPlaceResponse;
import com.project.webbanhang.services.Interfaces.IGoongService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@Slf4j
public class GoongService implements IGoongService {

    @Value("${goong.apiKey}")
    private String apiKey;

    private final GoongApi goongApi;

    @Override
    public GoongPlaceResponse searchPlaces(GoongPlaceSearchDTO searchDTO) {
        try {
            log.info("Searching places with input: {}", searchDTO.getInput());
            
            return goongApi.searchPlaces(
                    searchDTO.getInput(),
                    apiKey,
                    searchDTO.getLocation(),
                    searchDTO.getLimit() != null ? searchDTO.getLimit() : 10,
                    searchDTO.getRadius() != null ? searchDTO.getRadius() : 50,
                    searchDTO.getMoreCompound() != null ? searchDTO.getMoreCompound() : false
            );
        } catch (Exception e) {
            log.error("Error searching places with Goong API", e);
            throw new RuntimeException("Failed to search places: " + e.getMessage());
        }
    }
}
