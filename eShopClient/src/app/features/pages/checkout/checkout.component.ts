import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PaymentModel } from '../../models/payment';
import { AddressModel } from '../../models/address';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { AuthService } from '../../../core/services/auth.service';
import { AddressService } from '../../services/address.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Countries } from '../../constants/countries';
import { Cities } from '../../constants/cities';
import { Months } from '../../constants/months';
import { Years } from '../../constants/years';
import { AddAddressModel } from '../../models/addAddress';
import { UpdateAddressModel } from '../../models/updateAddress';


@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [TranslateModule, CommonModule, FormsModule],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent {

  shippingAddress!: AddressModel; 
  billingAddress!: AddressModel;
  newShippingAddress: AddAddressModel = {  userId: '',contactName: '',country: '',city: '',zipCode: '',description: ''};

  newBillingAddress: AddAddressModel = { userId: '',contactName: '',country: '',city: '', zipCode: '', description: ''
  };

   //currency: string = "";
   shippingAndCartTotal: string = "";
   paymentRequest: PaymentModel = new PaymentModel();
   countries = Countries;
   cities = Cities;
   months = Months;
   years = Years;
   isSameAddress: boolean = false;

  isShippingAddressAvailable: boolean = false; 
  isBillingAddressAvailable: boolean = false; 

  constructor(
    public shopping: ShoppingCartService,
    private addressService: AddressService,
    private auth: AuthService
  ) { 
    this.paymentRequest.address = this.paymentRequest.address || {};
    this.paymentRequest.billingAddress = this.paymentRequest.billingAddress || {};
    
 
  }

  ngOnInit(): void {
    this.checkAddresses();
  }

  checkAddresses() {
    
    this.addressService.get().subscribe({
      next: (address: AddressModel) => {
        if (address) {
          this.shippingAddress = address;
          this.isShippingAddressAvailable = true;
        } else {
          this.isShippingAddressAvailable = false;
        }
      },
      error: (err) => {
        console.error('Error fetching shipping address:', err);
        this.isShippingAddressAvailable = false;
      }
    });

    this.addressService.getBillingAddress().subscribe({
      next: (billingAddress: AddressModel) => {
        if (billingAddress) {
          this.billingAddress = billingAddress;
          this.isBillingAddressAvailable = true;
        } else {
          this.isBillingAddressAvailable = false;
        }
      },
      error: (err) => {
        console.error('Error fetching billing address:', err);
        this.isBillingAddressAvailable = false;
      }
    });
  }

  createShippingAddress() {
    this.newShippingAddress.userId = this.auth.token!.userId;
    this.addressService.createAddress(this.newShippingAddress).subscribe({
      next: (address: AddressModel) => {
        this.shippingAddress = address;
        this.isShippingAddressAvailable = true;
      },
      error: (err) => {
        console.error('Error creating shipping address:', err);
      }
    });
  }

  createBillingAddress() {
    this.newBillingAddress.userId = this.auth.token!.userId;
    this.addressService.createBillingAddress(this.newBillingAddress).subscribe({
      next: (billingAddress: AddressModel) => {
        this.billingAddress = billingAddress;
        this.isBillingAddressAvailable = true;
      },
      error: (err) => {
        console.error('Error creating billing address:', err);
      }
    });
  }

  payment() {

    
    this.paymentRequest.products = this.shopping.shoppingCarts;
    this.paymentRequest.shippingAndCartTotal = Number(localStorage.getItem("shipping&CartTotal"));
    //this.paymentRequest.currency = this.currency;
    const userId = this.auth.token!.userId;
    this.paymentRequest.userId = userId === null ? "null" : userId;

   
    if (this.isShippingAddressAvailable) {
        this.paymentRequest.address.country = this.shippingAddress.country;
        this.paymentRequest.address.city = this.shippingAddress.city;
        this.paymentRequest.address.contactName = this.shippingAddress.contactName;
        this.paymentRequest.address.zipCode = this.shippingAddress.zipCode;
        this.paymentRequest.address.description = this.shippingAddress.description;
    }

   
    if (this.isBillingAddressAvailable) {
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
        localStorage.removeItem("shipping&CartTotal");
        
        localStorage.setItem("paymentDetails", JSON.stringify(this.paymentRequest));
    });
}

selectUseSaveShippingAdress() {
    this.isShippingAddressAvailable = true;
}

selectAddNewShippingAdress() {
    this.isShippingAddressAvailable = false;
    this.newShippingAddress = { userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };
}

selectUseSaveBillingAdress() {
    this.isBillingAddressAvailable = true;
}

selectAddNewBillingAdress() {
    this.isBillingAddressAvailable = false;
    this.newBillingAddress = { userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };
}

changeIsSameAddress() {
    if (this.isSameAddress) {
        this.paymentRequest.billingAddress = { ...this.paymentRequest.address };
    }}

onIdentityNumberInput(event: any) {
    event.target.value = event.target.value.replace(/[^0-9]/g, '');
}

onPhoneNumberInput(event: any) {
    event.target.value = event.target.value.replace(/[^0-9!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/g, '');
}

onCardNumberInput(event: any) {
    event.target.value = event.target.value.replace(/[^0-9]/g, '');
}

onCardCvcNumberInput(event: any) {
    event.target.value = event.target.value.replace(/[^0-9]/g, '');
}

  
}
