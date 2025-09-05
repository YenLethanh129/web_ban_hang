import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Observable, tap, of } from 'rxjs';
import { RegisterDTO } from '../dtos/register.dto';
import { LoginDTO } from '../dtos/login.dto';
import { UserDTO } from '../dtos/user.dto';
import { TokenService } from './token.service';
import { WebEnvironment } from '../environments/WebEnvironment';
import { LoginResponse } from '../response/login.response';
import { CacheService } from './cache.service';
import { UpdateUserDTO } from '../dtos/update-user.dto';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = `${WebEnvironment.apiUrl}/users`;
  private currentUser: UserDTO | null = null;

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private cacheService: CacheService,
    private injector: Injector
  ) {
    // Load user from cache on service initialization
    this.loadUserFromCache();
  }

  register(registerDTO: RegisterDTO): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'vi',
    });
    return this.http.post(`${this.apiUrl}/register`, registerDTO, { headers });
  }

  login(loginDTO: LoginDTO): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/login`, loginDTO, {
        headers,
      })
      .pipe(
        tap((response: LoginResponse) => {
          // After successful login, get user info and cache it
          if (response.token) {
            this.tokenService.setToken(response.token);
            this.getUserFromServer().subscribe((user) => {
              this.setCurrentUser(user);
            });
          }
        })
      );
  }

  getUser(): Observable<UserDTO> {
    // Check cache first
    const cachedUser = this.cacheService.getUser();
    if (cachedUser) {
      return of(cachedUser);
    }

    // If not in cache, fetch from server
    return this.getUserFromServer().pipe(
      tap((user) => this.setCurrentUser(user))
    );
  }

  private getUserFromServer(): Observable<UserDTO> {
    console.log('Token:', this.tokenService.getToken());
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.tokenService.getToken()}`,
    });
    return this.http.post<UserDTO>(
      `${this.apiUrl}/profile`,
      {}, // body rỗng
      { headers } // options
    );
  }

  setCurrentUser(user: UserDTO) {
    this.currentUser = user;
    this.cacheService.setUser(user);
  }

  getUserName(): string {
    // Try to get from cache first, then from current user
    const cachedUser = this.cacheService.getUser();
    return cachedUser?.fullname || this.currentUser?.fullname || '';
  }

  // Get user as Observable for real-time updates
  getUserObservable(): Observable<UserDTO | null> {
    return this.cacheService.getUserObservable();
  }

  // Check if user is cached
  isUserCached(): boolean {
    return this.cacheService.isUserCached();
  }

  // Load user from cache on service initialization
  private loadUserFromCache(): void {
    const cachedUser = this.cacheService.getUser();
    if (cachedUser) {
      this.currentUser = cachedUser;
    }
  }

  // Clear user cache on logout
  logout(): void {
    this.currentUser = null;
    this.cacheService.clearUser();
    this.cacheService.clearAll(); // Clear all cache data
    this.tokenService.removeToken();

    // Clear user address cache as well - using injector to avoid circular dependency
    try {
      import('./user-address.service')
        .then((module) => {
          const userAddressService = this.injector.get(
            module.UserAddressService
          );
          userAddressService.clearUserAddress();
        })
        .catch((error) => {
          console.warn('Could not clear user address cache:', error);
        });
    } catch (error) {
      console.warn('Could not import user address service:', error);
    }

    // Also clear localStorage manually to ensure everything is cleared
    localStorage.removeItem('user');
    localStorage.removeItem('token');

    console.log('🚪 User logged out - all data cleared');
  }

  // Force refresh user from server
  refreshUser(): Observable<UserDTO> {
    return this.getUserFromServer().pipe(
      tap((user) => this.setCurrentUser(user))
    );
  }

  // Update user profile
  updateUser(updateDTO: UpdateUserDTO): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${this.tokenService.getToken()}`,
    });

    return this.http.patch(`${this.apiUrl}/update`, updateDTO, { headers });
  }
}
