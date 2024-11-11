import { Component, HostListener } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WishListService } from '../../../features/services/wish-list.service';
import { ProductListService } from '../../../features/services/product-list.service';
import { ShoppingCartService } from '../../../features/services/shopping-cart.service';




@Component({
  selector: 'app-middlebar',
  standalone: true,
  imports: [RouterModule, TranslateModule, CommonModule, FormsModule],
  templateUrl: './middlebar.component.html',
  styleUrl: './middlebar.component.scss'
})
export class MiddlebarComponent {

  responseInLocalStorage: any;  
  showWishListCollapse = false; 
  productFilter: string = '';


  ngOnInit() {
    if (localStorage.getItem('response')) {
      this.responseInLocalStorage = localStorage.getItem("response");
      this.auth.checkAuthentication();
    }
    this.auth.getUser();
    console.log(this.auth.token?.userId);
    
  }

  constructor(public auth: AuthService, private router: Router, public wishList: WishListService, public shoplistProducts: ProductListService, public shoppingCart: ShoppingCartService) {}
  

  toggleCollapse() {
    this.showWishListCollapse = !this.showWishListCollapse; 
  }

  onSearch(): void {
    this.router.navigate(['/search'], { queryParams: { filter: this.productFilter } });
  }

 

  
}
