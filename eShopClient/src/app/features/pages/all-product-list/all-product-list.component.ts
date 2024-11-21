import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { ProductModel } from '../../models/product';
import { ProductListService } from '../../services/product-list.service';

@Component({
  selector: 'app-all-product-list',
  standalone: true,
  imports: [TranslateModule, CommonModule],
  templateUrl: './all-product-list.component.html',
  styleUrl: './all-product-list.component.scss'
})
export class AllProductListComponent {

  product: ProductModel[]=[];

  constructor(private shopping: ProductListService){}

  ngOnInit(): void {
    this.shopping.getProducts().subscribe({
      next: (res) => {
        this.product = res; // Gelen ürünleri atayın
        console.log(this.product); // Kontrol için loglayın
      },
      error: (err) => {
        console.error("Ürünler alınırken bir hata oluştu:", err);
      }
    });
  }
  

  

}
