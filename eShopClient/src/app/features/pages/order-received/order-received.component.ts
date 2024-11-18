import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { OrderModel } from '../../models/order';
import { HttpClient } from '@angular/common/http';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { SwalService } from '../../../core/services/swal.service';
import { AuthService } from '../../../core/services/auth.service';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-order-received',
  standalone: true,
  imports: [TranslateModule, CommonModule,],
  templateUrl: './order-received.component.html',
  styleUrl: './order-received.component.scss'
})
export class OrderReceivedComponent {

  order!: OrderModel;
    total: number = 0;

    payment: any;
    productPrices: any;
    shippingPrice: any;
    
    

    constructor(
      private orderService: OrderService,
      private auth: AuthService,
    ) {
      
    }
  
    ngOnInit() {
     
      const paymentDetailsString = localStorage.getItem('paymentDetails');
      const productPricesString = localStorage.getItem('productPrices');
  
      if (paymentDetailsString) {
        this.payment = JSON.parse(paymentDetailsString);
      }
      if (productPricesString) {
        this.productPrices = JSON.parse(productPricesString);
      }
      if (localStorage.getItem('shippingPrice')) {
        this.shippingPrice = localStorage.getItem('shippingPrice');
        console.log(this.shippingPrice);
      }
  
      
      this.orderService.getOrderReceivedByUserId().subscribe({
        next: (res: any) => {
          this.order = res;
          console.log(this.order);
        },
      });
    }
  

    calcTotal(): number {
        this.total = 0;

        
            for (let i = 0; i < this.order.products.length; i++) {
               
                    this.total += (this.order.products[i].price.value * this.order.products[i].quantity);
                    console.log(this.total);
                  
            }
        
        return this.total;          
    }

}
