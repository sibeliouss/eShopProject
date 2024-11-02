import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SwalService } from '../../core/services/swal.service';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../core/services/auth.service';
import { ProductModel } from '../models/product';
import { WishListModel } from '../models/wishList';
import { forkJoin } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WishListService {

  wishList: any[] = [];
  constructor(
    private translate: TranslateService,
    private swal: SwalService,
    private http: HttpClient,
    private auth: AuthService
  ) { }

  checkLocalStorageForWishList(){
    if(localStorage.getItem('response')){
      this.auth.checkAuthentication();
      this.http.get("https://localhost:5123/api/WishLists/GetAllWishList/" + this.auth.token?.userId).subscribe(
        {
          next: (res: any) => {
            this.wishList = res;
          },
         
        }
      )
    }
    else{
      this.wishList = [];
      localStorage.setItem('wishList', JSON.stringify(this.wishList));
    }
  }

  addToWishList(product: ProductModel) {
    if(localStorage.getItem('response')) {

       const data: WishListModel= new WishListModel();
      data.productId = product.id;
      data.userId = this.auth.token!.userId;
      data.price = product.price;

      this.http.post("https://localhost:7120/api/WishLists/AddToWishList", data).subscribe({
        next: (res: any) => {
          this.wishList.push(product);
          localStorage.setItem('wishList', JSON.stringify(this.wishList));
          this.checkLocalStorageForWishList();

          this.translate.get("bookAddedToWishlist").subscribe(
            res => {
              this.swal.callToast(res, 'success');
            }
          )
        },
        
      });
    }
    else {
      forkJoin({
        title: this.translate.get("pleaseLoginToAddTheProductToFavorites"),
        confirm: this.translate.get("confirmButton")
      }).subscribe(res => {
        this.swal.callSwal2(res.title, res.confirm, () => {

        })
      })
    }
  }

}
