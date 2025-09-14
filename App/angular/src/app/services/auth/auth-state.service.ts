import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserDTO } from '../../dtos/user.dto';
import { StorageService } from '../core/storage.service';
import { TokenService } from '../auth/token.service';

export interface AuthState {
  isAuthenticated: boolean;
  user: UserDTO | null;
  token: string | null;
  lastActivity: Date | null;
}

/**
 * Centralized authentication state management
 * Single responsibility: maintain auth state and notify observers
 * Clean separation from API calls and business logic
 */
@Injectable({
  providedIn: 'root',
})
export class AuthStateService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly USER_KEY = 'current_user';

  // Single source of truth for auth state
  private authState$ = new BehaviorSubject<AuthState>({
    isAuthenticated: false,
    user: null,
    token: null,
    lastActivity: null,
  });

  constructor(
    private storage: StorageService,
    private tokenService: TokenService
  ) {
    this.initializeFromStorage();
  }

  /**
   * Get current auth state as observable
   */
  getAuthState(): Observable<AuthState> {
    return this.authState$.asObservable();
  }

  /**
   * Get current auth state snapshot
   */
  getCurrentState(): AuthState {
    return this.authState$.value;
  }

  /**
   * Check if user is currently authenticated
   */
  isAuthenticated(): boolean {
    return this.authState$.value.isAuthenticated;
  }

  /**
   * Get current user (null if not authenticated)
   */
  getCurrentUser(): UserDTO | null {
    return this.authState$.value.user;
  }

  /**
   * Get current token (null if not authenticated)
   */
  getCurrentToken(): string | null {
    return this.authState$.value.token;
  }

  /**
   * Set authentication state after successful login
   */
  setAuthenticatedState(token: string, user: UserDTO): void {
    // Validate token before setting state
    if (!this.tokenService.isTokenValid(token)) {
      console.warn('Attempting to set invalid token');
      return;
    }

    const newState: AuthState = {
      isAuthenticated: true,
      user,
      token,
      lastActivity: new Date(),
    };

    // Persist to storage
    this.storage.setItem(this.TOKEN_KEY, token);
    this.storage.setItem(this.USER_KEY, JSON.stringify(user));

    // Update state
    this.authState$.next(newState);

    console.log('✅ User authenticated successfully');
  }

  /**
   * Update user information (keep same token)
   */
  updateUser(user: UserDTO): void {
    const currentState = this.authState$.value;

    if (!currentState.isAuthenticated) {
      console.warn('Cannot update user - not authenticated');
      return;
    }

    const newState: AuthState = {
      ...currentState,
      user,
      lastActivity: new Date(),
    };

    // Update storage
    this.storage.setItem(this.USER_KEY, JSON.stringify(user));

    // Update state
    this.authState$.next(newState);
  }

  /**
   * Update last activity timestamp
   */
  updateLastActivity(): void {
    const currentState = this.authState$.value;

    if (!currentState.isAuthenticated) {
      return;
    }

    this.authState$.next({
      ...currentState,
      lastActivity: new Date(),
    });
  }

  /**
   * Clear authentication state (logout)
   */
  clearAuthState(): void {
    const newState: AuthState = {
      isAuthenticated: false,
      user: null,
      token: null,
      lastActivity: null,
    };

    // Clear storage
    this.storage.removeItem(this.TOKEN_KEY);
    this.storage.removeItem(this.USER_KEY);

    // Update state
    this.authState$.next(newState);

    console.log('🚪 Authentication state cleared');
  }

  /**
   * Check if current token is valid
   */
  isTokenValid(): boolean {
    const token = this.getCurrentToken();
    return token ? this.tokenService.isTokenValid(token) : false;
  }

  /**
   * Check if token is expiring soon
   */
  isTokenExpiringSoon(thresholdMs: number = 5 * 60 * 1000): boolean {
    const token = this.getCurrentToken();
    return token
      ? this.tokenService.isTokenExpiringSoon(token, thresholdMs)
      : false;
  }

  /**
   * Get time until token expires
   */
  getTimeUntilTokenExpiry(): number {
    const token = this.getCurrentToken();
    return token ? this.tokenService.getTimeUntilExpiry(token) : 0;
  }

  /**
   * Initialize auth state from storage on service startup
   */
  private initializeFromStorage(): void {
    const storedToken = this.storage.getItem(this.TOKEN_KEY);
    const storedUserJson = this.storage.getItem(this.USER_KEY);

    // If no stored data, keep default unauthenticated state
    if (!storedToken || !storedUserJson) {
      return;
    }

    // Validate stored token
    if (!this.tokenService.isTokenValid(storedToken)) {
      console.log('Stored token is invalid/expired, clearing storage');
      this.clearAuthState();
      return;
    }

    // Parse stored user data
    try {
      const storedUser: UserDTO = JSON.parse(storedUserJson);

      const restoredState: AuthState = {
        isAuthenticated: true,
        user: storedUser,
        token: storedToken,
        lastActivity: new Date(), // Reset to current time
      };

      this.authState$.next(restoredState);
      console.log('✅ Authentication state restored from storage');
    } catch (error) {
      console.error('Failed to parse stored user data:', error);
      this.clearAuthState();
    }
  }
}
