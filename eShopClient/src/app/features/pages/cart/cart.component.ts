import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ShoppingCartService } from '../../services/shopping-cart.service';


@Component({
  standalone: true,
  imports: [FormsModule, TranslateModule, CommonModule,RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent {
  currency: string = "";

  constructor( public shopping: ShoppingCartService,private cdRef: ChangeDetectorRef
   
    
){}

trackByFn(index: number, item: any): any {
  return item.id; // Her bir ürün için benzersiz bir ID döndürmelidir.
}


}
