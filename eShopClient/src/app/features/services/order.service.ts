import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { OrderModel } from '../models/order';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private apiUrl = 'https://localhost:7120/api/Orders/';

  constructor(
    private http: HttpClient,
    private auth: AuthService
  ) {}

  getOrdersByUserId(userId: string): Observable<OrderModel[]> {
    this.auth.checkAuthentication();
    return this.http.get<OrderModel[]>(`${this.apiUrl}GetAllOrdersByUserId/${userId}`);
  }

  delete(orderId: string): Observable<any> {
    this.auth.checkAuthentication();
    return this.http.delete(`${this.apiUrl}DeleteOrder/${orderId}`);
  }

  
  getOrderDetailsByUserId(userId: string, orderId: string): Observable<OrderModel[]> {
    return this.http.get<OrderModel[]>(`${this.apiUrl}GetOrderDetailsByUserId/${userId}/${orderId}`);
  }
}
