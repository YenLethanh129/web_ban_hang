import { Injectable } from '@angular/core';

export interface JwtPayload {
  sub?: string;
  iat?: number;
  exp?: number;
  [key: string]: any;
}

/**
 * Pure JWT token operations service
 * Single responsibility: decode, validate, extract info from JWT tokens
 * No dependencies on other services - can be used anywhere
 */
@Injectable({
  providedIn: 'root',
})
export class TokenService {
  /**
   * Decode JWT token to get payload
   * Returns null if token is invalid or malformed
   */
  decodeToken(token: string): JwtPayload | null {
    if (!token || typeof token !== 'string') {
      return null;
    }

    try {
      // JWT structure: header.payload.signature
      const parts = token.split('.');
      if (parts.length !== 3) {
        return null;
      }

      const payload = parts[1];
      const base64 = payload.replace(/-/g, '+').replace(/_/g, '/');

      // Add padding if needed
      const paddedBase64 = base64 + '='.repeat((4 - (base64.length % 4)) % 4);

      const jsonPayload = decodeURIComponent(
        atob(paddedBase64)
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );

      return JSON.parse(jsonPayload);
    } catch (error) {
      console.warn('Failed to decode JWT token:', error);
      return null;
    }
  }

  /**
   * Check if token is expired
   */
  isTokenExpired(token: string): boolean {
    const payload = this.decodeToken(token);
    if (!payload || !payload.exp) {
      return true; // Consider invalid tokens as expired
    }

    const currentTime = Math.floor(Date.now() / 1000);
    return payload.exp <= currentTime;
  }

  /**
   * Check if token is valid (not expired and properly formatted)
   */
  isTokenValid(token: string): boolean {
    if (!token) {
      return false;
    }

    const payload = this.decodeToken(token);
    if (!payload) {
      return false;
    }

    return !this.isTokenExpired(token);
  }

  /**
   * Get token expiry time as Date object
   */
  getTokenExpiryDate(token: string): Date | null {
    const payload = this.decodeToken(token);
    if (!payload || !payload.exp) {
      return null;
    }

    return new Date(payload.exp * 1000);
  }

  /**
   * Get remaining time until token expires (in milliseconds)
   */
  getTimeUntilExpiry(token: string): number {
    const expiryDate = this.getTokenExpiryDate(token);
    if (!expiryDate) {
      return 0;
    }

    const remainingTime = expiryDate.getTime() - Date.now();
    return Math.max(0, remainingTime);
  }

  /**
   * Check if token expires within specified time (in milliseconds)
   */
  isTokenExpiringSoon(
    token: string,
    thresholdMs: number = 5 * 60 * 1000
  ): boolean {
    const timeUntilExpiry = this.getTimeUntilExpiry(token);
    return timeUntilExpiry > 0 && timeUntilExpiry <= thresholdMs;
  }

  /**
   * Extract user info from token payload
   */
  getUserInfoFromToken(
    token: string
  ): { userId?: string; email?: string; roles?: string[] } | null {
    const payload = this.decodeToken(token);
    if (!payload) {
      return null;
    }

    return {
      userId: payload.sub || payload['user_id'] || payload['userId'],
      email: payload['email'],
      roles: payload['roles'] || payload['authorities'] || [],
    };
  }

  /**
   * Get token issuer
   */
  getTokenIssuer(token: string): string | null {
    const payload = this.decodeToken(token);
    return payload?.['iss'] || null;
  }

  /**
   * Get token issued at time
   */
  getTokenIssuedAt(token: string): Date | null {
    const payload = this.decodeToken(token);
    if (!payload || !payload.iat) {
      return null;
    }

    return new Date(payload.iat * 1000);
  }
}
