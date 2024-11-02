import { Component } from '@angular/core';
import { ProductListService } from '../../services/product-list.service';
import { CartService } from '../../services/cart.service';
import { WishListService } from '../../services/wish-list.service';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-list-by-category',
  standalone: true,
  imports: [TranslateModule,FormsModule, CommonModule],
  templateUrl: './product-list-by-category.component.html',
  styleUrl: './product-list-by-category.component.scss'
})
export class ProductListByCategoryComponent {

  
  currentMonth: string = "";

  constructor(
    public shopListProducts: ProductListService,
    public shopping: CartService,
    public wishList: WishListService,
    
  ){
    const currentDate = new Date();
     const monthNames = [
      "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
      "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
    ];
    const currentMonthIndex = currentDate.getMonth();
    this.currentMonth = monthNames[currentMonthIndex];
  }

}
