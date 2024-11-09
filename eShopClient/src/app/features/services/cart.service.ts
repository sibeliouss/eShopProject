import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SwalService } from '../../core/services/swal.service';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { ProductModel } from '../models/product';
import { AddShoppingCartModel } from '../models/addShoppingCart';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  shoppingCarts: ProductModel[] = [];
  prices: { value: number, currency: string }[] = [];
  selectedCurrency: string = '';
  count: number = 0;
  total: number = 0;
  cartTotal: number = 0;
  payBtnDisabled: boolean = false;

  constructor(private translate: TranslateService,
    private http: HttpClient, 
    private swal: SwalService,
     private router: Router, 
     private auth: AuthService) 
    { }


    NgOnInit(){
      this.GetAllCarts();
    }

    GetAllCarts(){
      if (localStorage.getItem('shoppingCarts')) {
        const carts: string | null = localStorage.getItem('shoppingCarts')
        if (carts !== null) {
          this.shoppingCarts = JSON.parse(carts);
        }
      } else {
        this.shoppingCarts = [];
      }
  
      //Kullanıcı varsa
      if (localStorage.getItem('response')) {
        this.auth.checkAuthentication();
        this.http.get("https://localhost:5123/api/Carts/GetAllCarts/" + this.auth.token?.userId).subscribe({
          next: (res: any) => {
            this.shoppingCarts = res;
           
          },
          
        });
      } 
    }

    calcTotal() {
      this.total = 0;
  
      const sumMap = new Map<string, number>();
  
      for (const s of this.shoppingCarts) {
        const price = { ...s.price };
        const quantity = s.quantity;
  
        // localStorage.setItem('quantity', JSON.stringify(quantity));
  
        
          price.value *= quantity;
        
  
        const currentSum = sumMap.get(price.currency) || 0;
        sumMap.set(price.currency, currentSum + price.value);
      }
  
      this.prices = Array.from(sumMap, ([currency, value]) => ({ currency, value }));
      localStorage.setItem('prices', JSON.stringify(this.prices));
    }

    addShoppingCart(product: ProductModel) {
      if (localStorage.getItem("response")) {
        const data: AddShoppingCartModel = new AddShoppingCartModel();
        data.productId = product.id;
        data.price = product.price;
        data.quantity = product.quantity;
        data.userId = this.auth.token!.userId;
  
        this.http.post("https://localhost:7120/api/Carts/CreateCart", data).subscribe({
          next: (res: any) => {
            this.GetAllCarts();
            this.calcTotal();
            localStorage.setItem("productPrices", JSON.stringify(this.prices));
  
            this.translate.get("productAddedtoCart").subscribe(
              res => {
                this.swal.callToast(res, 'success');
              });
          },
          
        })
      }
      else {
        console.log(product.quantity)
        if (product.quantity === 0) {
          this.translate.get("theProductIsOutOfStock").subscribe(res => {
            this.swal.callToast(res, 'error');
          });
        }
        else {
          const checkProductIsAlreadyExists = this.shoppingCarts.find(p => p.id == product.id);
          if (checkProductIsAlreadyExists !== undefined) {
            checkProductIsAlreadyExists.quantity += 1;
          } else {
            const newProduct = { ...product };
            newProduct.quantity = 1;
            this.shoppingCarts.push(newProduct);
          }
  
          localStorage.setItem('shoppingCarts', JSON.stringify(this.shoppingCarts));
          this.calcTotal();
          localStorage.setItem("productPrices", JSON.stringify(this.prices));
          product.quantity -= 1;
          this.translate.get("productAddedtoCart").subscribe(
            res => {
              this.swal.callToast(res, 'success');
            });
        }
      }
    }
  
  
  }
