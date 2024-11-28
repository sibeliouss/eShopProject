import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PaymentModel } from '../../models/payment';
import { AddressModel } from '../../models/address';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { AuthService } from '../../../core/services/auth.service';
import { AddressService } from '../../services/address.service';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Countries } from '../../constants/countries';
import { Cities } from '../../constants/cities';
import { Months } from '../../constants/months';
import { Years } from '../../constants/years';
import { AddAddressModel } from '../../models/addAddress';
import { UpdateAddressModel } from '../../models/updateAddress';
import { BaseInputErrorsComponent } from '../../../core/components/base-input-errors/base-input-errors.component';


@Component({
  
  standalone: true,
  imports: [TranslateModule, CommonModule, FormsModule, ReactiveFormsModule, BaseInputErrorsComponent],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  buyerForm!: FormGroup;
  creditCardForm!: FormGroup;
  shippingAddressForm!: FormGroup;
  billingAddressForm!: FormGroup;
  
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
    private auth: AuthService,private fb: FormBuilder
  ) { 
    this.paymentRequest.address = this.paymentRequest.address || {};
    this.paymentRequest.billingAddress = this.paymentRequest.billingAddress || {};
    
 
  }

  ngOnInit(): void {
    this.checkAddresses();
    this.initializeForm();

  }

  initializeForm() {
    this.buyerForm = this.fb.group({
      firstName: [this.paymentRequest.buyer.name, [Validators.required, Validators.minLength(2)]],
      lastName: [this.paymentRequest.buyer.surname, [Validators.required, Validators.minLength(2)]],
      identityNumber: [this.paymentRequest.buyer.identityNumber, [Validators.required,Validators.minLength(11),Validators.maxLength(11),Validators.pattern(/^\d+$/),], ],
      phoneNumber: [ this.paymentRequest.buyer.gsmNumber,[Validators.required,Validators.pattern(/^\d{10,15}$/), ],],
      email: [this.paymentRequest.buyer.email, [Validators.required, Validators.email]],
    });

    this.creditCardForm = this.fb.group({
      cardHolderName: [this.paymentRequest.paymentCard.cardHolderName, [Validators.required]],
      cardNumber: [this.paymentRequest.paymentCard.cardNumber, [Validators.required, Validators.pattern(/^\d{16}$/)]],
      expireYear: [this.paymentRequest.paymentCard.expireYear, [Validators.required]],
      expireMonth: [this.paymentRequest.paymentCard.expireMonth, [Validators.required]],
      cvc: [this.paymentRequest.paymentCard.cvc, [Validators.required, Validators.pattern(/^\d{3}$/)]],
    });
    this.shippingAddressForm = this.fb.group({
      contactName: [, [Validators.required]],
      country: [, [Validators.required]],
      city: [, [Validators.required]],
      zipCode: [, [Validators.required, Validators.pattern(/^\d{5}$/)]],
      description: [, [Validators.required]]
    });

    this.billingAddressForm = this.fb.group({
      billingContactName: [, [Validators.required]],
      billingDescription: [, [Validators.required]],
      billingCity: [, [Validators.required]],
      billingZipCode: [, [Validators.required, Validators.pattern(/^\d{5}$/) ]],
      billingCountry: [, [Validators.required]],
    });
  }

  checkAddresses() {

    this.addressService.get().subscribe({
      next: (address: AddressModel) => {
        if (address) {
          this.shippingAddress = address;
          this.isShippingAddressAvailable = true;
          this.shippingAddressForm.patchValue({
            contactName: this.shippingAddress.contactName,
            country: this.shippingAddress.country,
            city: this.shippingAddress.city,
            zipCode: this.shippingAddress.zipCode,
            description: this.shippingAddress.description
          });
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
          this.billingAddressForm.patchValue({
            billingContactName: this.billingAddress.contactName,
            billingCountry: this.billingAddress.country,
            billingCity: this.billingAddress.city,
            billingZipCode: this.billingAddress.zipCode,
            billingDescription: this.billingAddress.description
          });
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
    if (this.creditCardForm.valid) {
      const paymentDetails = { ...this.creditCardForm.value, ...this.paymentRequest };
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
      console.log('Payment Details:', paymentDetails);
    } else {
      console.error('Form is invalid!');
    }
  }


/* 
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
}  */

changeIsSameAddress() {
    if (this.isSameAddress) {
        this.paymentRequest.billingAddress = { ...this.paymentRequest.address };
    }}
    
    
    isAddressVisible: boolean = false;
    toggleAddressVisibility() {
      this.isAddressVisible = !this.isAddressVisible;
    }

  
}
