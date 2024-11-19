import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ProductModel } from '../../features/models/product';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ShoppingCartService } from '../../features/services/shopping-cart.service';
import { WishListService } from '../../features/services/wish-list.service';
import { ProductListService } from '../../features/services/product-list.service';
import { ProductDiscountModel } from '../../features/models/productDiscount';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule, TranslateModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  product: ProductModel[]=[];
  discountProducts: ProductDiscountModel[] = [];
  newArrivalProducts: ProductModel[] = [];
  featuredProducts: ProductModel[] = [];
  loading: boolean = true; 
  apiUrl: string = 'https://localhost:7120/api/Products'; 
  currentMonth: string = "";
  
  


  constructor(
      private http: HttpClient,
      public translate: TranslateService,
      public shopping: ShoppingCartService,
      public wishList: WishListService,
      public shopListProducts: ProductListService,
  ) {}

  ngOnInit(): void {
    this.getNewProducts();
    this.getFeaturedProducts();
    this.getProducts();
    this.getDiscountedProducts();
    this.clearLocalStorage();
  }

  clearLocalStorage(): void {
    localStorage.removeItem("paymentDetails");
    localStorage.removeItem("shippingAndCartTotal");
    localStorage.removeItem("shippingPrice");
    localStorage.removeItem("currency");
  }

  getProducts(): void {
    this.http.get<ProductModel[]>(`${this.apiUrl}/GetProducts`).subscribe({
        next: (res: ProductModel[]) => {
            this.product = res; // Tüm ürünler dizi olarak atanır
            console.log(this.product);
            this.loading = false;
        },
        error: (err) => {
            console.error('Error loading products', err);
            this.loading = false;
        }
    });
}


  getNewProducts(): void {
    this.http.get<ProductModel[]>(`${this.apiUrl}/GetNewArrivals`).subscribe({
        next: (res) => {
            this.newArrivalProducts = res;
            console.log(this.newArrivalProducts);
            this.loading = false;
        },
        error: (err) => {
            console.error('Error loading new products', err);
            this.loading = false;
        }
    });
  }

  getFeaturedProducts(): void {
      this.http.get<ProductModel[]>(`${this.apiUrl}/GetFeaturedProducts`).subscribe({
          next: (res) => {
              this.featuredProducts = res;
              console.log(this.featuredProducts);
              this.loading = false;
          },
          error: (err) => {
              console.error('Error loading featured products', err);
              this.loading = false;
          }
      });
  }

  getDiscountedProductsById(id: string): void {
    this.http.get<ProductDiscountModel[]>(`https://localhost:7120/api/ProductDiscounts/GetProductDiscountById/${id}`).subscribe({
        next: (res) => {
            this.discountProducts = res;
            console.log(this.discountProducts);
        },
        error: (err) => {
            console.error('Error loading discounted product by ID', err);
        }
    });
  }

  getDiscountedProducts(): void {
    
    this.http.get<ProductDiscountModel[]>(`https://localhost:7120/api/ProductDiscounts/GetAllProductDiscounts`).subscribe({
        next: (res) => {
            this.discountProducts = res;
            console.log(this.discountProducts);
        },
        error: (err) => {
            console.error('Error loading discounted products', err);
        }
    });
  }

}
