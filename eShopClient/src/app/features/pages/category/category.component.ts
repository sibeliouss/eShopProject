import { Component } from '@angular/core';
import { CategoryModel } from '../../models/category';
import { CategoryService } from '../../services/category.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './category.component.html',
  styleUrl: './category.component.scss'
})
export class CategoryComponent {

  categories: CategoryModel[] = []; 

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.categoryService.getCategories1().subscribe({
      next: (data) => this.categories = data,
      error: (err) => console.error('Kategori yüklenemedi', err)
    });
  }


}
