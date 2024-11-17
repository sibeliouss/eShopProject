import { Component, OnInit } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { OrderModel } from '../../../models/order';
import { AuthService } from '../../../../core/services/auth.service';
import { ShoppingCartService } from '../../../services/shopping-cart.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { OrderService } from '../../../services/order.service';  // OrderService importu
import { forkJoin } from 'rxjs';
import { SwalService } from '../../../../core/services/swal.service';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [TranslateModule, CommonModule, RouterModule],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit {

  orders: OrderModel[] = [];
  total: number = 0;

  constructor(
    private auth: AuthService,
    private shopping: ShoppingCartService,
    private router: Router,
    private orderService: OrderService ,
    private translate: TranslateService ,
    private swal: SwalService
  ) {
   
  }

  ngOnInit(): void {
    this.loadOrders();  
  }

  loadOrders(): void {
    const userId = this.auth.token?.userId;
    if (userId) {
      this.orderService.getOrdersByUserId(userId).subscribe({
        next: (res: OrderModel[]) => {
          this.orders = res;
        },
        error: (err) => {
          console.error('Error loading orders', err);
        }
      });
    }
  }

  deleteOrder(orderId: string): void {
   
    forkJoin({
      delete: this.translate.get("remove.doYouWantToDeleted"),
      cancel: this.translate.get("remove.cancelButton"),
      confirm: this.translate.get("remove.confirmButton")
    }).subscribe(res => {
     
      this.swal.callSwal(res.delete, res.cancel, res.confirm, () => {
        
        this.orderService.delete(orderId).subscribe({
          next: () => {
            this.loadOrders();  // Siparişleri yeniden yükleme
          },
          error: (err) => {
            console.error('Error deleting order', err);
          }
        });
      });
    });
  }
  

  logout(): void {
    localStorage.removeItem("response");
    this.shopping.GetAllCarts();
    location.href = "/";
  }
}
