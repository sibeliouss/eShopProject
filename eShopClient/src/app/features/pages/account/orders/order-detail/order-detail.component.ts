import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { OrderModel } from '../../../../models/order';
import { OrderService } from '../../../../services/order.service';
import { ShoppingCartService } from '../../../../services/shopping-cart.service';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../../core/services/auth.service';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [TranslateModule, CommonModule, RouterModule],
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss'],
})
export class OrderDetailComponent {
  paymentDetails: string = '';
  total: number = 0;
  addComment = false;

  orders: OrderModel[] = [];

  constructor(
    private orderService: OrderService,
    private activated: ActivatedRoute,
    public shopping: ShoppingCartService,
    private auth: AuthService
  ) {}

  ngOnInit() {
    this.activated.params.subscribe((res) => {
      const orderId = res['order-id'];
      const userId = this.auth.token?.userId;

      if (!userId || !orderId) {
        console.error('User ID veya Order ID eksik.');
        return;
      }

      this.orderService.getOrderDetailsByUserId(userId, orderId).subscribe({
        next: (res) => {
          console.log('API Yanıtı:', res);
          this.orders = Array.isArray(res) ? res : [res];
          if (this.orders.length > 0) {
            console.log('Siparişler Yüklendi:', this.orders);
            this.commentBtnHideorShow();
          } else {
            console.warn('Hiç sipariş bulunamadı.');
          }
        },
        error: (err) => {
          console.error('Sipariş detaylarını alırken hata oluştu:', err);
        }
      });
    });
  }

  calcTotal(): number {
    this.total = 0;
    if (this.orders.length > 0) {
      const products = this.orders[0]?.products || [];
      for (let product of products) {
      
          this.total += product.price.value * product.quantity;
        
      }
    }
    return this.total;
  }

  commentBtnHideorShow() {
    if (this.orders.length > 0 && this.orders[0]?.status === 'Teslim Edildi') {
      this.addComment = true;
    } else {
      this.addComment = false;
    }
  }
}
