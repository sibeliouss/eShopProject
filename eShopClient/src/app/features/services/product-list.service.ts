import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { RequestModel } from '../models/request';
import { ProductModel } from '../models/product';
import { CategoryModel } from '../models/category';
import { ResponseDto } from '../models/response';
import { Observable } from 'rxjs';
import { Star } from '../models/star';

@Injectable({
  providedIn: 'root'
})
export class ProductListService {
  response: ResponseDto<ProductModel[]> | null = null;
  product: ProductModel | null= null;

  pageNumbers: number[] = [];
  searchCategory: string = "";
  request: RequestModel = new RequestModel();
  products: ProductModel[] = [];
  category: CategoryModel[] = [];
  starData: Star = new Star;

  
 // rating: number = 0;
  


  constructor(
    private router: Router,
    private http: HttpClient,
    private translate: TranslateService
  ) {
    this.getAllCategories();
    this.getAllProducts();
  }

  goToShopListByCategoryId(categoryId: string) {
    this.request.categoryId = categoryId;
    this.router.navigate(['/shop-list', categoryId]);
    this.getAllCategories();
    this.getAllProducts();
  }

  getAllProducts(pageNumber: number = 1): void {
    this.request.pageNumber = pageNumber;

    // RequestModel'in değerlerini kontrol et
    console.log('Request:', this.request);

    this.http.post<ResponseDto<ProductModel[]>>(`https://localhost:7120/api/Products/GetAllProducts`, this.request)
      .subscribe({
        next: (res) => {
          this.response = res;
          this.setPageNumber();
        },
        error: (error) => {
          console.error("Ürünleri alırken bir hata oluştu: ", error);
         
        }
      });
}



getProducts(): Observable<ProductModel[]> {
  return this.http.get<ProductModel[]>('https://localhost:7120/api/Products/GetProducts');
}

getAllCategories(): void {
    this.http.get<CategoryModel[]>('https://localhost:7120/api/Categories/GetAllCategories')
      .subscribe({
        next: (res) => {
          this.category = res;
        },
        error: (error) => {
          console.error("Kategorileri alırken bir hata oluştu: ", error);
        }
      });
  }

  setPageNumber() {
    this.pageNumbers = [];
    if (this.response && this.response.totalPageCount) {
      for (let i = 0; i < this.response.totalPageCount; i++) {
        this.pageNumbers.push(i + 1);
      }
    }
  }
  
   changeSorting(): void {
    this.getAllProducts();
  }

  changePageSize(): void {
    this.getAllProducts(1);
  }

  
  
  
} 