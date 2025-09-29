import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, Injector, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import {
  Observable,
  tap,
  of,
  retry,
  map,
  catchError,
  BehaviorSubject,
} from 'rxjs';
import { RegisterDTO } from '../dtos/register.dto';
import { LoginRequestDTO } from '../dtos/login.dto';
import { UserDTO } from '../dtos/user.dto';
import { TokenService } from './token.service';
import { WebEnvironment } from '../environments/WebEnvironment';
import { LoginResponse } from '../dtos/login.dto';
import { CacheService } from './cache.service';
import { UpdateUserDTO } from '../dtos/update-user.dto';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = `${WebEnvironment.apiUrl}/users`;
  private currentUser: UserDTO | null = null;

  // Track authentication state
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  private userSubject = new BehaviorSubject<UserDTO | null>(null);

  // Public observables
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  public user$ = this.userSubject.asObservable();

  constructor(
    private http: HttpClient,
    private cacheService: CacheService,
    private injector: Injector,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    // Khởi tạo trạng thái xác thực khi ứng dụng khởi động
    this.initializeAuthState();
  }

  register(registerDTO: RegisterDTO): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'vi',
    });
    return this.http.post(`${this.apiUrl}/register`, registerDTO, { headers });
  }

  login(request: LoginRequestDTO): Observable<LoginResponse> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'vi',
    });

    return this.http
      .post<LoginResponse>(`${this.apiUrl}/login`, request, {
        headers,
        withCredentials: true,
      })
      .pipe(
        retry(1), // Thử lại 1 lần nếu thất bại
        tap((response) => {
          if (response) {
            this.getUserFromServer().subscribe({
              next: (reponse) => {
                this.setCurrentUser(reponse);
                this.isAuthenticatedSubject.next(true);
              },
              error: (error) => {
                console.error('❌ Error loading user info after login:', error);
              },
            });
          }
        }),
        catchError((error) => {
          console.error('❌ Login error:', error);
          throw error;
        })
      );
  }

  // Khởi tạo trạng thái xác thực khi ứng dụng khởi động
  private initializeAuthState(): void {
    if (isPlatformBrowser(this.platformId)) {
      // Kiểm tra cache trước
      const cachedUser = this.cacheService.getUser();
      if (cachedUser) {
        this.setCurrentUser(cachedUser);
        this.isAuthenticatedSubject.next(true);
      } else {
      }

      // Luôn kiểm tra với server để đảm bảo
      this.checkAuthenticationStatus().subscribe({
        next: (isAuth) => {},
        error: (error) => {
          console.error('❌ UserService: Initial auth check failed:', error);
        },
      });
    } else {
    }
  }

  // Kiểm tra nếu người dùng đã xác thực
  checkAuthenticationStatus(): Observable<boolean> {
    return this.http
      .post<any>(
        `${this.apiUrl}/profile`,
        {},
        {
          withCredentials: true,
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest',
          }),
        }
      )
      .pipe(
        tap((response) => {
          if (response && response.user) {
            this.setCurrentUser(response.user);
            this.isAuthenticatedSubject.next(true);
          }
        }),
        map((response) => !!(response && response.user)),
        catchError((error) => {
          console.error('❌ Authentication check error:', error);

          // Log chi tiết để debug
          if (error.status === 403) {
            console.log(
              '🚫 Forbidden - JWT cookie might be missing or invalid'
            );
          } else if (error.status === 401) {
          } else if (error.status === 400) {
          }

          return of(false);
        })
      );
  }

  getUser(): Observable<UserDTO> {
    // Check cache first
    const cachedUser = this.cacheService.getUser();
    if (cachedUser) {
      return of(cachedUser);
    }

    return this.getUserFromServer().pipe(
      tap((user) => this.setCurrentUser(user))
    );
  }

  // Force refresh user from server
  refreshUserSync(): void {
    this.getUserFromServer()
      .pipe(tap((user) => this.setCurrentUser(user)))
      .subscribe();
  }

  private getUserFromServer(): Observable<UserDTO> {
    return this.http
      .post<any>(
        `${this.apiUrl}/profile`,
        {},
        {
          withCredentials: true,
          headers: new HttpHeaders({
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest',
          }),
        }
      )
      .pipe(
        map((response) => {
          if (response && response.user) {
            return response.user as UserDTO;
          }
          throw new Error(
            'Invalid user response format - missing userResponse'
          );
        }),
        catchError((error) => {
          console.error('❌ Error getting user from server:', error);
          throw error;
        })
      );
  }

  setCurrentUser(user: UserDTO) {
    this.currentUser = user;
    this.userSubject.next(user);
    this.cacheService.setUser(user);

    console.log('👤 Current user set:', {
      fullname: user.fullname,
      phone: user.phone_number,
      address: user.address,
    });
  }

  // Kiểm tra trạng thái xác thực hiện tại
  isAuthenticated(): boolean {
    const isAuth = this.isAuthenticatedSubject.value;
    console.log('🔍 UserService: isAuthenticated() =', isAuth);
    return isAuth;
  }

  // Lấy thông tin người dùng hiện tại một cách đồng bộ
  getCurrentUser(): UserDTO | null {
    const user = this.userSubject.value;
    console.log('👤 UserService: getCurrentUser() =', user?.fullname || 'null');
    return user;
  }

  // Clear user cache on logout
  logout(): Observable<any> {
    // Gọi API logout backend trước
    return this.logoutFromServer().pipe(
      tap((response) => {}),
      catchError((error) => {
        console.warn(
          '⚠️ Server logout failed, continuing with local cleanup:',
          error
        );
        // Vẫn tiếp tục cleanup local dù server logout fail
        return of({
          success: false,
          message: 'Server logout failed but local cleanup will proceed',
        });
      }),
      tap(() => {
        // Thực hiện cleanup local sau khi gọi server (dù thành công hay thất bại)
        this.performLocalCleanup();
      })
    );
  }

  // Gọi API logout từ server
  private logoutFromServer(): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/logout`,
      {},
      {
        withCredentials: true,
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'X-Requested-With': 'XMLHttpRequest',
        }),
      }
    );
  }

  private performLocalCleanup(): void {
    // Clear authentication state
    this.currentUser = null;
    this.userSubject.next(null);
    this.isAuthenticatedSubject.next(false);

    // Clear all cache services
    // Clear only user-related cache and pagination metadata.
    // Keep product cache intact so offline/product browsing still works.
    this.cacheService.clearUser();
    this.cacheService.clearPaginationData();

    // Clear user address cache - chỉ trong browser environment
    if (isPlatformBrowser(this.platformId)) {
      try {
        // Dynamic import user address service để clear
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

      // Clear localStorage completely
      try {
        // Specific keys first
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        localStorage.removeItem('cart');
        localStorage.removeItem('userAddress');
        localStorage.removeItem('searchHistory');
        localStorage.removeItem('recentProducts');

        // Clear all app-related keys (keeping browser settings)
        const keysToRemove: string[] = [];
        for (let i = 0; i < localStorage.length; i++) {
          const key = localStorage.key(i);
          if (
            key &&
            (key.startsWith('app_') ||
              key.startsWith('user_') ||
              key.startsWith('cart_') ||
              key.startsWith('order_') ||
              key.includes('coffee') ||
              key.includes('auth'))
          ) {
            keysToRemove.push(key);
          }
        }

        keysToRemove.forEach((key) => {
          localStorage.removeItem(key);
        });
      } catch (error) {
        console.warn('Could not access localStorage:', error);
      }

      // Clear sessionStorage
      try {
        sessionStorage.clear();
      } catch (error) {
        console.warn('Could not access sessionStorage:', error);
      }
    }
  }

  // Force refresh user from server
  refreshUser(): Observable<UserDTO> {
    return this.getUserFromServer().pipe(
      tap((user) => this.setCurrentUser(user))
    );
  }

  // Update user profile
  updateUser(updateDTO: UpdateUserDTO): Observable<any> {
    return this.http.patch(`${this.apiUrl}/update`, updateDTO, {
      withCredentials: true,
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest',
      }),
    });
  }

  // Update user password
  updatePassword(passwordData: {
    old_password: string;
    new_password: string;
  }): Observable<any> {
    return this.http.post(`${this.apiUrl}/update-password`, passwordData, {
      withCredentials: true,
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest',
      }),
    });
  }
}
