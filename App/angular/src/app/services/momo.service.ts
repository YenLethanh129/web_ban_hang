import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WebEnvironment } from '../environments/WebEnvironment';
import { MomoInfoOrderDTO, OrderDTO } from '../dtos/order.dto';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class MomoService {
    private apiUrl = `${WebEnvironment.momoUrl}/create`;
    private currentUser: OrderDTO | null = null;

    constructor(private http: HttpClient) {}

    createQR(momoInfoOrderDTO: MomoInfoOrderDTO): Observable<any> {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept-Language': 'vi',
          });
        return this.http.post(`${this.apiUrl}`, momoInfoOrderDTO,{ headers });
    }

}