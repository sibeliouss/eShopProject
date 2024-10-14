import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Customer } from '../models/Customer';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  private baseUrl: string = 'http://localhost:5123/api/Customers';

  constructor(private http: HttpClient) { }

  getCustomerByUserId(userId: string): Observable<Customer> {
    const url = `${this.baseUrl}/GetCustomerByUserId/${userId}`;
    return this.http.get<Customer>(url);
  }

  getCustomerById(customerId: string): Observable<Customer> {
    const url = `${this.baseUrl}/GetCustomerById/${customerId}`;
    return this.http.get<Customer>(url);
  }
}
