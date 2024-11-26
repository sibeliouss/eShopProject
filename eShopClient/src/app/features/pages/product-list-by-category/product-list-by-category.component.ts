 import { Component } from '@angular/core';


 import { WishListService } from '../../services/wish-list.service';
 import { TranslateModule } from '@ngx-translate/core';
 import { CommonModule } from '@angular/common';
 import { FormsModule } from '@angular/forms';
import { ProductListService } from '../../services/product-list.service';
import { RouterLink, RouterModule } from '@angular/router';
import { ProductModel } from '../../models/product';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { CategoryModel } from '../../models/category';

 @Component({
  standalone: true,
   imports: [TranslateModule,FormsModule, CommonModule, RouterModule],
   templateUrl: './product-list-by-category.component.html',
  styleUrl: './product-list-by-category.component.scss'
 })
 export class ProductListByCategoryComponent {

  categoy: CategoryModel|null= null;

   products: ProductModel[]=[];
   constructor(
     public shopListProducts: ProductListService,
  
     public wishList: WishListService, public shopping: ShoppingCartService
    
   ){} 


}





 
