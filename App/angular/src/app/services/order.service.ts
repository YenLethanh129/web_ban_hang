import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WebEnvironment } from '../environments/WebEnvironment';
import { OrderDTO } from '../dtos/order.dto';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class OrderService {
    private apiUrl = `${WebEnvironment.apiUrl}/orders`;
    private currentUser: OrderDTO | null = null;

    constructor(private http: HttpClient) {}

    createOrder(orderDTO: OrderDTO): Observable<any> {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept-Language': 'vi',
          });
        return this.http.post(`${this.apiUrl}`, orderDTO, { headers });
    }
}