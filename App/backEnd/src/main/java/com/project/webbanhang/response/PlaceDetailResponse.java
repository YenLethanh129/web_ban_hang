package com.project.webbanhang.response;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

/**
 *
 {
 "result": {
 "place_id": "Hobn8WqBW6rsKtKq2PDrVKp4BJNRtiILxTQbB__muXgRB3v8GRDTfkp_6lc4cbLw/5PUgWrMDrSI/xlqDBt5XA==.ZXhwYW5kMA==",
 "formatted_address": "Phường Trung Hòa, Quận Cầu Giấy, Thành phố Hà Nội",
 "geometry": {
 "location": {
 "lat": 21.0137625240001,
 "lng": 105.798267363
 }
 },
 "name": "91 Trung Kính"
 },
 "status": "OK"
 }

 */

@Data
@AllArgsConstructor
@NoArgsConstructor
public class PlaceDetailResponse {
    private Place result;
    private String status;

    @Data
    @AllArgsConstructor
    @NoArgsConstructor
    public static class Place {
        private String place_id;
        private String formatted_address;
        private Geometry geometry;
        private String name;

        @Data
        @AllArgsConstructor
        @NoArgsConstructor
        public static class Geometry {
            private Location location;

            @Data
            @AllArgsConstructor
            @NoArgsConstructor
            public static class Location {
                private Double lat;
                private Double lng;
            }
        }
    }
}
