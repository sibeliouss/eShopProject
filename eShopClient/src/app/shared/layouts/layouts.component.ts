import { Component } from '@angular/core';
import { TopbarComponent } from "./topbar/topbar.component";
import { MiddlebarComponent } from "./middlebar/middlebar.component";
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FooterComponent } from "./footer/footer.component";
import { CategoryModel } from '../../features/models/category';
import { CategoryService } from '../../features/services/category.service';
import { TranslateModule } from '@ngx-translate/core';
import { ProductListService } from '../../features/services/product-list.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-layouts',
  standalone: true,
  imports: [TopbarComponent, MiddlebarComponent, CommonModule, TranslateModule],
  templateUrl: './layouts.component.html',
  styleUrl: './layouts.component.scss'
})
export class LayoutsComponent {

  showBars: boolean = true;
  categories: CategoryModel[] = [];

  constructor(public router: Router, private categoryService: CategoryService, public productList: ProductListService, private http: HttpClient) {
   
    this.router.events.subscribe((event: any) => {
      if (event.url) {
        // Hem login hem de register sayfalarında gizlenmesi için kontrol
        this.showBars = !(event.url.includes('/login') || event.url.includes('/register'));
      }
    });
  }

  ngOnInit() {
    this.getBrowseCategories1();
  }

  getBrowseCategories1(){
    this.categoryService.getCategories1().subscribe((data)=>{
     this.categories=data;
      
    })
  }

  trackById(index: number, item: CategoryModel): string {
    return item.id;
  }
  

}
