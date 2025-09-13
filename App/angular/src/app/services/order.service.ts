import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WebEnvironment } from '../environments/WebEnvironment';
import {
  OrderDTO,
  OrderResponseDTO,
  NewOrderResponseDTO,
} from '../dtos/order.dto';
import { Observable } from 'rxjs';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private apiUrl = `${WebEnvironment.apiUrl}/orders`;
  private currentUser: OrderDTO | null = null;

  constructor(private http: HttpClient, private tokenService: TokenService) {}

  createOrder(orderDTO: OrderDTO): Observable<any> {
    const token = this.tokenService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'vi',
      Authorization: `Bearer ${token}`,
    });
    return this.http.post(`${this.apiUrl}`, orderDTO, { headers });
  }

  getOrderById(orderId: number): Observable<OrderResponseDTO> {
    const token = this.tokenService.getToken();
    const headers = new HttpHeaders({
      'Accept-Language': 'vi',
      Authorization: `Bearer ${token}`,
    });
    return this.http.get<OrderResponseDTO>(`${this.apiUrl}/${orderId}`, {
      headers,
    });
  }

  getOrdersByUserId(userId: number): Observable<OrderResponseDTO[]> {
    const token = this.tokenService.getToken();
    const headers = new HttpHeaders({
      'Accept-Language': 'vi',
      Authorization: `Bearer ${token}`,
    });
    return this.http.get<OrderResponseDTO[]>(`${this.apiUrl}/user/${userId}`, {
      headers,
    });
  }

  // New method for getting user orders using the new API endpoint
  getUserOrders(): Observable<NewOrderResponseDTO[]> {
    const token = this.tokenService.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'vi',
      Authorization: `Bearer ${token}`,
    });
    // API yêu cầu POST method, không phải GET
    return this.http.post<NewOrderResponseDTO[]>(
      `${this.apiUrl}/user`,
      {},
      {
        headers,
      }
    );
  }
}
