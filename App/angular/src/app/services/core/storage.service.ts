import { Injectable } from '@angular/core';
import { PlatformService } from './platform.service';

export interface StorageConfig {
  prefix?: string;
  fallbackToSessionStorage?: boolean;
}

/**
 * Clean, simple storage service with proper error handling
 * Single responsibility: handle browser storage operations
 */
@Injectable({
  providedIn: 'root',
})
export class StorageService {
  private readonly defaultConfig: StorageConfig = {
    prefix: 'app_',
    fallbackToSessionStorage: true,
  };

  constructor(private platform: PlatformService) {}

  /**
   * Store data in localStorage with validation
   */
  setItem(key: string, value: string, config?: StorageConfig): boolean {
    if (!this.platform.isBrowser()) {
      console.warn('Storage operation attempted in non-browser environment');
      return false;
    }

    const settings = { ...this.defaultConfig, ...config };
    const prefixedKey = `${settings.prefix}${key}`;

    // Validate inputs
    if (!key?.trim() || typeof value !== 'string') {
      console.warn('StorageService: Invalid key or value', { key, value });
      return false;
    }

    // Don't store null/undefined as strings
    if (value === 'null' || value === 'undefined' || !value.trim()) {
      console.warn('StorageService: Attempting to store invalid value', {
        key,
        value,
      });
      return false;
    }

    // Try localStorage first
    if (this.platform.hasLocalStorage()) {
      try {
        localStorage.setItem(prefixedKey, value);
        return true;
      } catch (error) {
        console.error('localStorage failed:', error);

        // Fallback to sessionStorage if configured
        if (
          settings.fallbackToSessionStorage &&
          this.platform.hasSessionStorage()
        ) {
          try {
            sessionStorage.setItem(prefixedKey, value);
            return true;
          } catch (sessionError) {
            console.error('sessionStorage fallback failed:', sessionError);
          }
        }
      }
    }

    return false;
  }

  /**
   * Retrieve data from storage with validation
   */
  getItem(key: string, config?: StorageConfig): string | null {
    if (!this.platform.isBrowser() || !key?.trim()) {
      return null;
    }

    const settings = { ...this.defaultConfig, ...config };
    const prefixedKey = `${settings.prefix}${key}`;

    // Try localStorage first
    if (this.platform.hasLocalStorage()) {
      try {
        const value = localStorage.getItem(prefixedKey);
        return this.validateStoredValue(value, prefixedKey);
      } catch (error) {
        console.error('localStorage read failed:', error);
      }
    }

    // Fallback to sessionStorage
    if (
      settings.fallbackToSessionStorage &&
      this.platform.hasSessionStorage()
    ) {
      try {
        const value = sessionStorage.getItem(prefixedKey);
        return this.validateStoredValue(value, prefixedKey);
      } catch (error) {
        console.error('sessionStorage read failed:', error);
      }
    }

    return null;
  }

  /**
   * Remove item from storage
   */
  removeItem(key: string, config?: StorageConfig): void {
    if (!this.platform.isBrowser() || !key?.trim()) {
      return;
    }

    const settings = { ...this.defaultConfig, ...config };
    const prefixedKey = `${settings.prefix}${key}`;

    if (this.platform.hasLocalStorage()) {
      try {
        localStorage.removeItem(prefixedKey);
      } catch (error) {
        console.error('localStorage remove failed:', error);
      }
    }

    if (this.platform.hasSessionStorage()) {
      try {
        sessionStorage.removeItem(prefixedKey);
      } catch (error) {
        console.error('sessionStorage remove failed:', error);
      }
    }
  }

  /**
   * Clear all items with the configured prefix
   */
  clear(config?: StorageConfig): void {
    if (!this.platform.isBrowser()) {
      return;
    }

    const settings = { ...this.defaultConfig, ...config };
    const prefix = settings.prefix;

    // Clear localStorage
    if (this.platform.hasLocalStorage()) {
      try {
        const keysToRemove: string[] = [];
        for (let i = 0; i < localStorage.length; i++) {
          const key = localStorage.key(i);
          if (key && key.startsWith(prefix!)) {
            keysToRemove.push(key);
          }
        }
        keysToRemove.forEach((key) => localStorage.removeItem(key));
      } catch (error) {
        console.error('localStorage clear failed:', error);
      }
    }

    // Clear sessionStorage
    if (this.platform.hasSessionStorage()) {
      try {
        const keysToRemove: string[] = [];
        for (let i = 0; i < sessionStorage.length; i++) {
          const key = sessionStorage.key(i);
          if (key && key.startsWith(prefix!)) {
            keysToRemove.push(key);
          }
        }
        keysToRemove.forEach((key) => sessionStorage.removeItem(key));
      } catch (error) {
        console.error('sessionStorage clear failed:', error);
      }
    }
  }

  /**
   * Validate stored value and clean up invalid data
   */
  private validateStoredValue(
    value: string | null,
    key: string
  ): string | null {
    if (value === null) {
      return null;
    }

    // Clean up invalid string representations
    if (value === 'null' || value === 'undefined') {
      console.warn(`Found invalid stored value for ${key}, cleaning up`);
      localStorage.removeItem(key);
      sessionStorage.removeItem(key);
      return null;
    }

    return value;
  }
}
