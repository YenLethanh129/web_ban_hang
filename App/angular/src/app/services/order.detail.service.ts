import { Injectable } from '@angular/core';
import { OrderDetailDTO, OrderDetailResponseDTO } from '../dtos/order.dto';
import { WebEnvironment } from '../environments/WebEnvironment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class OrderDetailService {
  private apiUrl = `${WebEnvironment.apiUrl}/order_details`;
  private currentUser: OrderDetailDTO | null = null;

  constructor(private http: HttpClient, private tokenService: TokenService) {}

  createOrderDetail(orderDetailDTO: OrderDetailDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}`, orderDetailDTO, {
      withCredentials: true,
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest',
      }),
    });
  }

  getOrderDetails(orderId: number): Observable<OrderDetailResponseDTO[]> {
    return this.http.get<OrderDetailResponseDTO[]>(
      `${this.apiUrl}/order/${orderId}`,
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
