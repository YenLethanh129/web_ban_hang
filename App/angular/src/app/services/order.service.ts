import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WebEnvironment } from '../environments/WebEnvironment';
import {
  OrderDTO,
  OrderResponseDTO,
  NewOrderResponseDTO,
  OrderConfirmResponseDTO,
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
    return this.http.post(`${this.apiUrl}`, orderDTO, {
      withCredentials: true,
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest',
      }),
    });
  }

  getUserOrders(): Observable<NewOrderResponseDTO[]> {
    return this.http.post<NewOrderResponseDTO[]>(
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
}
