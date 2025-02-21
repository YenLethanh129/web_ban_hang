import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegisterDTO } from '../dtos/register.dto';
import { LoginDTO } from '../dtos/login.dto';
import { UserDTO } from '../dtos/user.dto';
import { TokenService } from './token.service';
import { WebEnvironment } from '../environments/WebEnvironment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = `${WebEnvironment.apiUrl}/users`;
  private currentUser: UserDTO | null = null;

  constructor(private http: HttpClient, private tokenService: TokenService) {}

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
    return this.http.post(`${this.apiUrl}/login`, loginDTO, {
      headers,
      responseType: 'text',
    });
  }

  getUser(): Observable<UserDTO> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.tokenService.getToken()}`
    );
    return this.http.get<UserDTO>(`${this.apiUrl}/profile`, { headers });
  }

  setCurrentUser(user: UserDTO) {
    this.currentUser = user; // Biến user lấy thông tin từ tham số đầu vào của hàm setCurrentUser
  }

  getUserName(): string {
    return this.currentUser?.fullname || '';
  }
}
