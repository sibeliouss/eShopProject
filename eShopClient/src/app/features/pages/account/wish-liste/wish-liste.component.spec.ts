import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WishListeComponent } from './wish-liste.component';

describe('WishListeComponent', () => {
  let component: WishListeComponent;
  let fixture: ComponentFixture<WishListeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WishListeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WishListeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
