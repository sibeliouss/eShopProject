import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PaymentModel } from '../../models/payment';
import { Countries } from '../../constants/countries';
import { Cities } from '../../constants/cities';
import { Months } from '../../constants/months';
import { Years } from '../../constants/years';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { AuthService } from '../../../core/services/auth.service';
import { AddressService } from '../../services/address.service';
import { AddressModel } from '../../models/address';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [TranslateModule, CommonModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent {

  currency: string = "";
  shippingAndCartTotal: string = "";
  paymentRequest: PaymentModel|null=null;
  countries = Countries;
  cities = Cities;
  months = Months;
  years = Years;
  isSameAddress: boolean = false;

  shippingAddress!: AddressModel;
  billingAddress!: AddressModel;

  isAvailableShippingAddress = false;
  isAvailableBillingAddress = false;

  newShippingAddress = false;
  newBillingAddress = false;

  constructor(
      public shopping: ShoppingCartService,
      private auth: AuthService,
      public address: AddressService
  ) {
      
      }
  

      payment() {
        if (!this.paymentRequest) {
          console.error('PaymentRequest is null or undefined!');
          return;
        }
      
        this.paymentRequest.products = this.shopping.shoppingCarts;
        this.paymentRequest.shippingAndCartTotal = Number(localStorage.getItem("shipping&CartTotal"));
        this.paymentRequest.currency = this.currency;
      
        const userId = this.auth.token?.userId ?? "null"; 
        this.paymentRequest.userId = userId;
      
        if (this.isAvailableShippingAddress) {
          this.paymentRequest.address.country = this.shippingAddress.country;
          this.paymentRequest.address.city = this.shippingAddress.city;
          this.paymentRequest.address.contactName = this.shippingAddress.contactName;
          this.paymentRequest.address.zipCode = this.shippingAddress.zipCode;
          this.paymentRequest.address.description = this.shippingAddress.description;
        }
      
        if (this.isAvailableBillingAddress) {
          this.paymentRequest.billingAddress.country = this.billingAddress.country;
          this.paymentRequest.billingAddress.city = this.billingAddress.city;
          this.paymentRequest.billingAddress.contactName = this.billingAddress.contactName;
          this.paymentRequest.billingAddress.zipCode = this.billingAddress.zipCode;
          this.paymentRequest.billingAddress.description = this.billingAddress.description;
        }
      
        this.shopping.payment(this.paymentRequest, (res) => {
          localStorage.removeItem("shoppingCarts");
          this.shopping.shoppingCarts = [];
          this.shopping.shoppingCarts.length = 0;
          localStorage.removeItem("shippingAndCartTotal");
          localStorage.setItem("paymentDetails", JSON.stringify(this.paymentRequest));
        });
      }
      

   
}
 