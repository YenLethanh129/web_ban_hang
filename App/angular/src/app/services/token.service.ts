import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { StorageService } from './storage.service';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private readonly TOKEN_KEY = 'auth_token';

  constructor(
    private storageService: StorageService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  setToken(token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      this.storageService.setItem(this.TOKEN_KEY, token);
    }
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return this.storageService.getItem(this.TOKEN_KEY);
    }
    return null;
  }

  removeToken(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.storageService.removeItem(this.TOKEN_KEY);
    }
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }
}
