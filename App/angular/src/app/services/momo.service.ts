import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WebEnvironment } from '../environments/WebEnvironment';
import { MomoInfoOrderDTO, OrderDTO } from '../dtos/order.dto';
import { Observable } from 'rxjs';
import { MomoIpnRequestDTO } from '../dtos/momo.dto';

@Injectable({
  providedIn: 'root',
})
export class MomoService {
  private apiUrl = `${WebEnvironment.momoUrl}`;
  private currentUser: OrderDTO | null = null;

  constructor(private http: HttpClient) {}

  createQR(momoInfoOrderDTO: MomoInfoOrderDTO): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'vi',
    });
    return this.http.post(`${this.apiUrl}/create`, momoInfoOrderDTO, {
      headers,
    });
  }

  ipnHandler(momoIpnRequestDTO: MomoIpnRequestDTO): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept-Language': 'vi',
    });
    return this.http.post(`${this.apiUrl}/ipnHandler`, momoIpnRequestDTO, {
      headers,
    });
  }
}
