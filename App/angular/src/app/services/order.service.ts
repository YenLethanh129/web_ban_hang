import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WebEnvironment } from '../environments/WebEnvironment';
import {
  OrderRequestDTO,
  OrderResponseDTO,
  OrderConfirmResponseDTO,
} from '../dtos/order.dto';
import { Observable } from 'rxjs';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private apiUrl = `${WebEnvironment.apiUrl}/orders`;
  private currentUser: OrderRequestDTO | null = null;

  constructor(private http: HttpClient, private tokenService: TokenService) {}

  createOrder(orderDTO: OrderRequestDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}`, orderDTO, {
      withCredentials: true,
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest',
      }),
    });
  }

  getUserOrders(): Observable<OrderResponseDTO[]> {
    return this.http.post<OrderResponseDTO[]>(
      `${this.apiUrl}/user`,
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

  getOrderConfirm(orderId: number): Observable<OrderConfirmResponseDTO> {
    return this.http.get<OrderConfirmResponseDTO>(`${this.apiUrl}/${orderId}`, {
      withCredentials: true,
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest',
      }),
    });
  }

  cancelOrder(order_id: number): Observable<any> {
    return this.http.put(
      `${this.apiUrl}/cancel/${order_id}`,
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
}
