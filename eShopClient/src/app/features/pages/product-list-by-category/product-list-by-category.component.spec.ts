import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductListByCategoryComponent } from './product-list-by-category.component';

describe('ProductListByCategoryComponent', () => {
  let component: ProductListByCategoryComponent;
  let fixture: ComponentFixture<ProductListByCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductListByCategoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductListByCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
