import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { CategoryModel } from '../../../features/models/category';
import { Router, RouterLink } from '@angular/router';
import { CategoryService } from '../../../features/services/category.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-bottombar',
  standalone: true,
  imports: [CommonModule, TranslateModule,RouterLink],
  templateUrl: './bottombar.component.html',
  styleUrl: './bottombar.component.scss'
})
export class BottombarComponent {

  categories: CategoryModel[] = [];

  constructor(public router: Router, private categoryService: CategoryService, private http: HttpClient) {
   
    
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
