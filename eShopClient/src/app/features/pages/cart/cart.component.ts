import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ShoppingCartService } from '../../services/shopping-cart.service';


@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [FormsModule, TranslateModule, CommonModule,RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent {
  currency: string = "";

  constructor( public shopping: ShoppingCartService
 
){}





trackByFn(index: number, item: any): number {
  return item.id;  // ID'yi kullanarak her öğeyi benzersiz şekilde takip ediyoruz
}
}
