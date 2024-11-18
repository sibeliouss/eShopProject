import { Injectable } from '@angular/core';
import { ProductModel } from '../models/product';
import { TranslateService } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { SwalService } from '../../core/services/swal.service';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { AddShoppingCartModel } from '../models/addShoppingCart';
import { PaymentModel } from '../models/payment';
import { forkJoin } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {

  shoppingCarts: ProductModel[] = [];
  count: number = 0;
  total: number = 0;
  cartTotal: number = 0;
  payBtnDisabled: boolean = false;
  flatRateTl: number = 50; //kargo ücreti
  //currency="";

  constructor(private translate: TranslateService,
              private http: HttpClient, 
              private swal: SwalService,
              private auth: AuthService, private router: Router) {
    this.GetAllCarts();
    this.shippingAndCartTotal();
  }

  GetAllCarts() {
    if (localStorage.getItem('shoppingCarts')) {
      const carts: string | null = localStorage.getItem('shoppingCarts')
      if (carts !== null) {
        this.shoppingCarts = JSON.parse(carts);
      }
    } else {
      this.shoppingCarts = [];
    }
  
    if (localStorage.getItem('response')) {
      this.auth.checkAuthentication();
      this.http.get("https://localhost:7120/api/Carts/GetAllCart/" + this.auth.token?.userId).subscribe({
        next: (res: any) => {
          this.shoppingCarts = res;
        },
        error: (err) => {
          console.error("Error in GetAllCarts:", err);
        }
      });
    }
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
          

          console.log('Product price before saving to localStorage:', this.total);
  
          localStorage.setItem("productPrices", JSON.stringify([{ currency: 'TRY', value: this.total }]));
          
          console.log('Product price saved to localStorage:', localStorage.getItem('productPrices'));
  
          this.translate.get("productAddedtoCart").subscribe(
            res => {
              this.swal.callToast(res, 'success');
            });
        },
        error: (err) => {
          console.error("Error in addShoppingCart:", err);
          this.translate.get("errorAddingProductToCart").subscribe(res => {
            this.swal.callToast(res, 'error');
          });
        }
      });
    } else {
      if (product.quantity === 0) {
        this.translate.get("theProductIsOutOfStock").subscribe(res => {
          this.swal.callToast(res, 'error');
        });
      } else {
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
  
        console.log('Product price before saving to localStorage:', this.total);
        localStorage.setItem("productPrices", JSON.stringify([{ currency: 'TRY', value: this.total }]));
        console.log('Product price saved to localStorage:', localStorage.getItem('productPrices'));
  
        product.quantity -= 1;
        this.translate.get("productAddedtoCart").subscribe(
          res => {
            this.swal.callToast(res, 'success');
          });
      }
    }
  }
  
  changeProductQuantityInCart(productId: string, quantity: number) {
    if (localStorage.getItem('response')) {
      const body = { productId, quantity }; 
      this.http.get(`https://localhost:7120/api/Carts/ChangeProductQuantityInCart/${productId}/${quantity}`).subscribe({
        next: (res: any) => {
          this.GetAllCarts();
        },
        error: (err) => {
          console.error("Error in changeProductQuantityInCart:", err);
          this.translate.get("errorUpdatingQuantity").subscribe(res => {
            this.swal.callToast(res, 'error');
          });
        }
      });
    } else {
      const product = this.shoppingCarts.find(p => p.id == productId);
      if (product !== undefined) {
        if (quantity == 0) {
          const productIndex = this.shoppingCarts.findIndex(p => p.id === productId);
          if (productIndex !== -1) {
            this.removeByIndex(productIndex);
            return;
          }
        } else {
          this.http.get(`https://localhost:7120/api/Carts/CheckProductQuantityIsAvailable/${productId}/${quantity}`).subscribe({
            next: (res: any) => {
              product.quantity = quantity;
              localStorage.setItem('shoppingCarts', JSON.stringify(this.shoppingCarts));
            },
          });
        }
      }
    }
  }
  

  removeByIndex(index: number) {
    forkJoin({
      delete: this.translate.get("remove.doYouWantToDeleted"),
      cancel: this.translate.get("remove.cancelButton"),
      confirm: this.translate.get("remove.confirmButton")
    }).subscribe(res => {
      this.swal.callSwal(res.delete, res.cancel, res.confirm, () => {
        if (localStorage.getItem("response")) {
          this.http.delete("https://localhost:7120/api/Carts/DeleteCart/" + this.shoppingCarts[index]?.cartId).subscribe({
            next: (res: any) => {
              this.GetAllCarts();
            },
          });
        } else {
          this.shoppingCarts.splice(index, 1);
          localStorage.setItem("shoppingCarts", JSON.stringify(this.shoppingCarts));
          this.count = this.shoppingCarts.length;
          this.calcTotal();
          this.shippingControl();
          
         
          console.log('Shopping cart before removing item:', this.shoppingCarts);
          console.log('Shopping cart after removing item:', this.shoppingCarts);
  
          localStorage.setItem("productPrices", JSON.stringify([{ currency: 'TRY', value: this.total }]));
        }
      });
    })
  }
  

  payment(data: PaymentModel, callBack: (res: any) => void) {
    this.http.post(`https://localhost:7120/api/Carts/Payment`, data).subscribe({
      next: (res: any) => {
        callBack(res);
        this.translate.get("paymentSuccessful").subscribe(
          res => {
            this.swal.callToast(res, 'success');
          }
        );
        setTimeout(() => {
          this.router.navigate(['/order-received']);
        }, 2000);
      },
      error: (err) => {
        console.error("Error in payment:", err);
        this.translate.get("paymentError").subscribe(res => {
          this.swal.callToast(res, 'error');
        });
      }
    });
  }

  shippingAndCartTotal(): number {
    this.cartTotal = this.getTotal();  
  
   
    console.log('Cart total before shipping fee:', this.cartTotal);
  
    if (this.getTotal() <= 500) {
      this.cartTotal += this.flatRateTl; 
    }
  
  
    console.log('Cart total after shipping fee:', this.cartTotal);
  
    localStorage.setItem('shipping&CartTotal', JSON.stringify(this.cartTotal));
    return this.cartTotal;
  }
  
  
  calcTotal() {
    this.total = 0;
  
    for (const s of this.shoppingCarts) {
      const price = s.price.value * s.quantity;
      this.total += price;
    }
  
    console.log('Total before saving to localStorage:', this.total);
  
   
    localStorage.setItem('prices', JSON.stringify([{ currency: 'TRY', value: this.total }]));
  

    console.log('Total saved to localStorage:', localStorage.getItem('prices'));
  }
  

  shippingControl() {
    const isFreeShipping: boolean = (this.getTotal() > 500);
  
    if (!isFreeShipping) {
      this.updateTotal('flatRate');
    }
  }

  updateTotal(shippingMethod: string): void {
    if (shippingMethod === 'flatRate') {
      this.total = this.flatRateTl; // TL cinsinden kargo ücreti
    }
  }

  getTotal() {
    let total = 0;
    this.shoppingCarts.forEach(item => {
      total += item.price.value * item.quantity;
    });
  
    
    console.log('Total before returning:', total);
  
    return total; 
  }
  
  
}