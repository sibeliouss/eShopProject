import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryModel } from '../models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private apiUrl = 'https://localhost:7120/api/Categories/GetAllCategories';

  constructor(private http: HttpClient) {}

  getCategories1(): Observable<CategoryModel[]> {
    return this.http.get<CategoryModel[]>(this.apiUrl);
}
}
