import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';

/**
 * Centralized platform detection service
 * Replaces scattered isPlatformBrowser checks throughout the app
 */
@Injectable({
  providedIn: 'root',
})
export class PlatformService {
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  /**
   * Check if running in browser environment
   */
  isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  /**
   * Check if running in server environment (SSR)
   */
  isServer(): boolean {
    return isPlatformServer(this.platformId);
  }

  /**
   * Check if localStorage is available
   */
  hasLocalStorage(): boolean {
    if (!this.isBrowser()) {
      return false;
    }

    try {
      const test = '__localStorage_test__';
      localStorage.setItem(test, 'test');
      localStorage.removeItem(test);
      return true;
    } catch {
      return false;
    }
  }

  /**
   * Check if sessionStorage is available
   */
  hasSessionStorage(): boolean {
    if (!this.isBrowser()) {
      return false;
    }

    try {
      const test = '__sessionStorage_test__';
      sessionStorage.setItem(test, 'test');
      sessionStorage.removeItem(test);
      return true;
    } catch {
      return false;
    }
  }
}
