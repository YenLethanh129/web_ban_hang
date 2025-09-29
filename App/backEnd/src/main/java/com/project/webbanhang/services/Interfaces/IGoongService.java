package com.project.webbanhang.services.Interfaces;

import com.project.webbanhang.dtos.goong.GoongPlaceSearchDTO;
import com.project.webbanhang.response.GoongPlaceResponse;
import com.project.webbanhang.response.PlaceDetailResponse;

public interface IGoongService {
    GoongPlaceResponse searchPlaces(GoongPlaceSearchDTO searchDTO);

    PlaceDetailResponse getPlaceDetails(String placeId);
}
