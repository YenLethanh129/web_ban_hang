package com.project.webbanhang.controllers;

import com.project.webbanhang.dtos.goong.GoongPlaceSearchDTO;
import com.project.webbanhang.response.GoongPlaceResponse;
import com.project.webbanhang.response.PlaceDetailResponse;
import com.project.webbanhang.services.Interfaces.IGoongService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("${api.prefix}/location")
@RequiredArgsConstructor
@Slf4j
public class GoongController {

    private final IGoongService goongService;

    @GetMapping("/search")
    public ResponseEntity<?> searchPlaces(
            @RequestParam String input,
            @RequestParam(required = false) String location,
            @RequestParam(defaultValue = "10") Integer limit,
            @RequestParam(defaultValue = "500") Integer radius,
            @RequestParam(defaultValue = "false") Boolean moreCompound
    ) {
        try {
            GoongPlaceSearchDTO searchDTO = GoongPlaceSearchDTO.builder()
                    .input(input)
                    .location(location)
                    .limit(limit)
                    .radius(radius)
                    .moreCompound(moreCompound)
                    .build();

            GoongPlaceResponse response = goongService.searchPlaces(searchDTO);
            
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            log.error("Error searching places", e);
            return ResponseEntity.badRequest().body("Search failed: " + e.getMessage());
        }
    }

    @GetMapping("/details")
    public ResponseEntity<?> getPlaceDetails(@RequestParam(name = "place_id", required = true) String placeId) {
        try {
            PlaceDetailResponse response = goongService.getPlaceDetails(placeId);
            return ResponseEntity.ok(response);
        } catch (Exception e) {
            log.error("Error getting place details", e);
            return ResponseEntity.badRequest().body("Get details failed: " + e.getMessage());
        }
    }
}
