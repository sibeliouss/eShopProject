import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProductModel } from '../models/product';
import { Observable } from 'rxjs';
import { ProductDiscountModel } from '../models/productDiscount';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl = 'https://localhost:7120/api/Products';
  private discountApiUrl = 'https://localhost:7120/api/ProductDiscounts';

  constructor(private http: HttpClient) {}

  getProducts(): Observable<ProductModel[]> {
    return this.http.get<ProductModel[]>(`${this.apiUrl}/GetProducts`);
  }

  getNewArrivals(): Observable<ProductModel[]> {
    return this.http.get<ProductModel[]>(`${this.apiUrl}/GetNewArrivals`);
  }

  getFeaturedProducts(): Observable<ProductModel[]> {
    return this.http.get<ProductModel[]>(`${this.apiUrl}/GetFeaturedProducts`);
  }

  getDiscountedProducts(): Observable<ProductDiscountModel[]> {
    return this.http.get<ProductDiscountModel[]>(`${this.discountApiUrl}/GetAllProductDiscounts`);
  }

  getDiscountedProductById(id: string): Observable<ProductDiscountModel[]> {
    return this.http.get<ProductDiscountModel[]>(`${this.discountApiUrl}/GetProductDiscountById/${id}`);
  }
 
}
