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
import { WishListModel } from '../../features/models/wishList';
import { ProductService } from '../../features/services/product.service';

@Component({
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule, TranslateModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  product: ProductModel[]=[];
  discountProducts: ProductDiscountModel[] = [];
  newArrivalProducts: ProductModel[] = [];
  featuredProducts: ProductModel[] = [];
  loading: boolean = true;  
  currentMonth: string = "";
  rating:number=0;
  wish:WishListModel=new WishListModel();

  constructor(
      private http: HttpClient,
      public translate: TranslateService,
      public shopping: ShoppingCartService,
      public wishList: WishListService,
      public shopListProducts: ProductListService,
      private productService: ProductService,

  ) {}

  ngOnInit(): void {
    this.getNewProducts();
    this.getFeaturedProducts();
    this.getProducts();
    this.getDiscountedProducts();
    this.clearLocalStorage();
   
  }
  

  isFavorite(item: ProductModel): boolean {
    return this.wishList.wishListItems.some(w => w.id === item.id);
  }
 
  

  clearLocalStorage(): void {
    localStorage.removeItem("paymentDetails");
    localStorage.removeItem("shippingAndCartTotal");
    localStorage.removeItem("shippingPrice");
    localStorage.removeItem("currency");
  }

  getProducts(): void {
    this.productService.getProducts().subscribe({
      next: (res) => {
        this.product = res;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading products', err);
        
      }
    });
  }

  getNewProducts(): void {
    this.productService.getNewArrivals().subscribe({
      next: (res) => {
        this.newArrivalProducts = res;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading new products', err);
        this.loading = false;
      }
    });
  }

  getFeaturedProducts(): void {
    this.productService.getFeaturedProducts().subscribe({
      next: (res) => {
        this.featuredProducts = res;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading featured products', err);
        this.loading = false;
      }
    });
  }

  getDiscountedProductsById(id: string): void {
    this.productService.getDiscountedProductById(id).subscribe({
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
    this.productService.getDiscountedProducts().subscribe({
      next: (res) => {
        const currentTime = new Date().getTime();
        this.discountProducts = res.filter(discount => {
          const endTime = new Date(discount.endDate).getTime();
          return endTime > currentTime; // Sadece aktif indirimleri al
        });
        console.log(this.discountProducts);
      },
      error: (err) => {
        console.error('Error loading discounted products', err);
      }
    });
  }
  

  calculateTimeLeft(endDate: string): string {
    const currentTime = new Date().getTime();
    const endTime = new Date(endDate).getTime();
    const timeDiff = endTime - currentTime;
  
    if (timeDiff <= 0) {
      return this.translate.instant('timeExpired'); // "Süre doldu" veya "Time expired"
    }
  
    const days = Math.floor(timeDiff / (1000 * 60 * 60 * 24));
    const hours = Math.floor((timeDiff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((timeDiff % (1000 * 60 * 60)) / (1000 * 60));
  
   
    return this.translate.instant('timeLeft', {
      days: days,
      hours: hours,
      minutes: minutes
    });
  }

  addToCart(product: ProductModel): void {
    if (product) {
        this.shopping.addShoppingCart(product);
    }
  }

  
  
   
  }
  
  
  


