package com.project.webbanhang.services;

import com.project.webbanhang.dtos.GoongPlaceSearchDTO;
import com.project.webbanhang.response.GoongPlaceResponse;

public interface IGoongService {
    GoongPlaceResponse searchPlaces(GoongPlaceSearchDTO searchDTO);
}
