import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProductListService } from '../../services/product-list.service';
import { CommonModule } from '@angular/common';
import { ProductModel } from '../../models/product';
import { TranslateModule } from '@ngx-translate/core';
import { ShoppingCartService } from '../../services/shopping-cart.service';

@Component({
  standalone: true,
  imports: [CommonModule, TranslateModule, RouterModule],
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {

  
  product: ProductModel|null=null;
  productList: ProductModel[] = [];
  filteredProducts: ProductModel[] = [];
  productFilter: string = '';

  constructor(
    private productService: ProductListService, 
    private router: Router, 
    private activatedRoute: ActivatedRoute,
    private shopping: ShoppingCartService
  ) {}

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.productFilter = params['filter'] || '';
      this.getProducts();
    });
  }

  getProducts(): void {
    this.productService.getProducts().subscribe((products: ProductModel[]) => {
      this.productList = products;
      this.filteredProducts = this.filterProducts(products);
    });
  }

  filterProducts(products: ProductModel[]): ProductModel[] {
    if (!this.productFilter) {
      return products;
    }
    return products.filter(product =>
      product.name.toLowerCase().includes(this.productFilter.toLowerCase()) ||
      product.brand.toLowerCase().includes(this.productFilter.toLowerCase()) ||
      product.productDetail.material.toLowerCase().includes(this.productFilter.toLowerCase())
    );
  }
  
  
  

 
}
