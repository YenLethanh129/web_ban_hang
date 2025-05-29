import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegisterDTO } from '../dtos/register.dto';
import { LoginDTO } from '../dtos/login.dto';
import { UserDTO } from '../dtos/user.dto';
import { TokenService } from './token.service';
import { WebEnvironment } from '../environments/WebEnvironment';
import { LoginResponse } from '../response/login.response';

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
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, loginDTO, {
      headers,
    });
  }

  getUser() {
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
  }

  getUserName(): string {
    return this.currentUser?.fullname || '';
  }
}
