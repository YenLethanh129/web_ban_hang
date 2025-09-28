package com.project.webbanhang.response;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class GoongPlaceResponse {
    private List<GoongPlace> predictions;
    private String status;
    @JsonProperty("executed_time")
    private Integer executedTime;
    @JsonProperty("executed_time_all")
    private Integer executedTimeAll;

    @Data
    @AllArgsConstructor
    @NoArgsConstructor
    public static class GoongPlace {
        private String description;
        @JsonProperty("place_id")
        private String placeId;
        private String reference;
        @JsonProperty("structured_formatting")
        private StructuredFormatting structuredFormatting;
        @JsonProperty("plus_code")
        private PlusCode plusCode;
        private Double score;
        @JsonProperty("has_children")
        private Boolean hasChildren;
        @JsonProperty("display_type")
        private String displayType;

        @Data
        @AllArgsConstructor
        @NoArgsConstructor
        public static class StructuredFormatting {
            @JsonProperty("main_text")
            private String mainText;
            @JsonProperty("secondary_text")
            private String secondaryText;
        }

        @Data
        @AllArgsConstructor
        @NoArgsConstructor
        public static class PlusCode {
            @JsonProperty("compound_code")
            private String compoundCode;
            @JsonProperty("global_code")
            private String globalCode;
        }
    }
}
