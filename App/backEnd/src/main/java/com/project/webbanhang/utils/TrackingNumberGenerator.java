package com.project.webbanhang.utils;

import java.util.Random;

public class TrackingNumberGenerator {
    private static final String ALPHANUMERIC_CHARS = "0123456789";

    public static String generateRandomTrackingNumber(int length) {
        if (length <= 0) {
            throw new IllegalArgumentException("Length must be a positive integer.");
        }

        Random random = new Random();
        StringBuilder sb = new StringBuilder(length);

        for (int i = 0; i < length; i++) {
            int randomIndex = random.nextInt(ALPHANUMERIC_CHARS.length());
            sb.append(ALPHANUMERIC_CHARS.charAt(randomIndex));
        }

        return sb.toString();
    }
}
