import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WebEnvironment } from '../environments/WebEnvironment';
import { OrderDTO } from '../dtos/order.dto';
import { Observable } from 'rxjs';
import { TokenService } from './token.service';

@Injectable({
    providedIn: 'root',
})
export class OrderService {
    private apiUrl = `${WebEnvironment.apiUrl}/orders`;
    private currentUser: OrderDTO | null = null;

    constructor(
        private http: HttpClient,
        private tokenService: TokenService
    ) {}

    createOrder(orderDTO: OrderDTO): Observable<any> {
        const token = this.tokenService.getToken();
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept-Language': 'vi',
            'Authorization': `Bearer ${token}`
        });
        return this.http.post(`${this.apiUrl}`, orderDTO, { headers });
    }

}