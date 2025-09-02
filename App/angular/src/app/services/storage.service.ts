import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  setItem(key: string, value: string): void {
    if (isPlatformBrowser(this.platformId)) {
      // Validate that key and value are proper strings
      if (typeof key !== 'string' || typeof value !== 'string') {
        console.warn('StorageService: Invalid key or value type', {
          key,
          value,
        });
        return;
      }

      // Don't store undefined or null strings
      if (value === 'undefined' || value === 'null' || value.trim() === '') {
        console.warn('StorageService: Attempting to store invalid value', {
          key,
          value,
        });
        return;
      }

      try {
        localStorage.setItem(key, value);
      } catch (error) {
        console.error('StorageService: Failed to store item', error);
      }
    }
  }

  getItem(key: string): string | null {
    if (isPlatformBrowser(this.platformId)) {
      if (typeof key !== 'string') {
        console.warn('StorageService: Invalid key type', { key });
        return null;
      }

      try {
        const value = localStorage.getItem(key);

        // Don't return string representations of undefined/null
        if (value === 'undefined' || value === 'null') {
          this.removeItem(key); // Clean up invalid data
          return null;
        }

        return value;
      } catch (error) {
        console.error('StorageService: Failed to get item', error);
        return null;
      }
    }
    return null;
  }

  removeItem(key: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(key);
    }
  }
}
