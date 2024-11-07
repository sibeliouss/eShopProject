import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { SwalService } from '../../core/services/swal.service';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../core/services/auth.service';
import { WishListModel } from '../models/wishList';
import { forkJoin } from 'rxjs';
import { ProductModel } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class WishListService {

  wishListItems: ProductModel[] = [];
  
  constructor(
    private translate: TranslateService,
    private swal: SwalService,
    private http: HttpClient,
    private auth: AuthService
  ) { this.checkLocalStorageForWishList(); }

  

  checkLocalStorageForWishList(){
    if(localStorage.getItem('response')){
      this.auth.checkAuthentication();
      this.http.get("https://localhost:7120/api/WishLists/GetAllWishLists/" + this.auth.token?.userId).subscribe({
        next: (res: any) => {
          this.wishListItems = res;
        },
        error: (err) => {
          console.error('Error fetching wish list:', err);
        
        }
      });
    }
    else{
      this.wishListItems = [];
      localStorage.setItem('wishList', JSON.stringify(this.wishListItems));
    }
  }

  addToWishList(product: ProductModel) {
    if(localStorage.getItem('response')) {

      const data: WishListModel = new WishListModel();
      data.productId = product.id;
      data.userId = this.auth.token?.userId ?? "";
      data.price = product.price;

      this.http.post("https://localhost:7120/api/WishLists/AddToWishList", data).subscribe({
        next: (res: any) => {
          this.wishListItems.push(product);
          localStorage.setItem('wishList', JSON.stringify(this.wishListItems));
          this.checkLocalStorageForWishList();

          this.translate.get("productAddedToWishlist").subscribe(
            res => {
              this.swal.callToast(res, 'success');
            }
          )
        },
        error: (err) => {
          console.error('Error adding product to wish list:', err);
        }
      });
    }
    else {
      forkJoin({
        title: this.translate.get("pleaseLoginToAddTheProductToFavorites"),
        confirm: this.translate.get("confirmButton")
      }).subscribe(res => {
        this.swal.callSwal2(res.title, res.confirm, () => {})
      })
    }
  }

  DeleteWishList(index: number) {
    forkJoin({
      delete: this.translate.get("remove.doYouWantToDeleted"),
      cancel: this.translate.get("remove.cancelButton"),
      confirm: this.translate.get("remove.confirmButton")
    }).subscribe(res => {
      this.swal.callSwal(res.delete, res.cancel, res.confirm, () => {

        if (localStorage.getItem("response")) {
          this.http.delete("https://localhost:7120/api/WishLists/Delete/" + this.wishListItems[index]?.wishListId).subscribe({
            next: (res: any) => {
              this.checkLocalStorageForWishList();
            },
            error: (err) => {
              console.error('Error deleting product from wish list:', err);
           
            }
          });
        }
      });
    })
  }
}
