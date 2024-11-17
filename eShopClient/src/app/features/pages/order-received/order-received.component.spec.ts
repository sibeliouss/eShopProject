import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderReceivedComponent } from './order-received.component';

describe('OrderReceivedComponent', () => {
  let component: OrderReceivedComponent;
  let fixture: ComponentFixture<OrderReceivedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderReceivedComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderReceivedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
