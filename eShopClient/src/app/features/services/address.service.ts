import { Injectable } from '@angular/core';
import { AddAddressModel } from '../models/addAddress';
import { UpdateAddressModel } from '../models/updateAddress';
import { Cities } from '../constants/cities';
import { Countries } from '../constants/countries';
import { AddressModel } from '../models/address';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../core/services/auth.service';
import { TranslateService } from '@ngx-translate/core';
import { SwalService } from '../../core/services/swal.service';

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  isShippingAddress: boolean = false;
  isBillingAddress: boolean = false;
  isaddShippingAddress: boolean = false;
  isaddBillingAddress: boolean = false;

  countries = Countries;
  cities = Cities;

  shippingRequestAdd: AddAddressModel = { userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };
  shippingRequestUpdate: UpdateAddressModel = { id: '', userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };
  billingRequestAdd: AddAddressModel = { userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };
  billingRequestUpdate: UpdateAddressModel = { id: '', userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };

  shippingAddress!: AddressModel;
  billingAddress!: AddressModel;

  addressId: string = "";
  contactName: string = "";
  country: string = "";
  city: string = "";
  zipCode: string = "";
  description: string = "";

  billingAddressId: string = "";
  billingContactName: string = "";
  billingCountry: string = "";
  billingCity: string = "";
  billingZipCode: string = "";
  billingDescription: string = "";

  constructor(
    private http: HttpClient,
    private auth: AuthService,
    private translate: TranslateService,
    private swal: SwalService
  ) {
    this.get();
    this.getBillingAddress();
  }

  addShippingAddress() {
    this.isaddShippingAddress = true;
  }

  addBillingAddress() {
    this.isaddBillingAddress = true;
  }

  editShippingAddress() {
    this.isShippingAddress = !this.isShippingAddress;
  }

  editBillingAddress() {
    this.isBillingAddress = !this.isBillingAddress;
  }

  addButton() {
    this.editShippingAddress();
    this.create();
  }

  addBillingButton() {
    this.editBillingAddress();
    this.createBillingAddress();
  }

  updateButton() {
    this.editShippingAddress();
    this.update();
  }

  updateBillingButton() {
    this.editBillingAddress();
    this.updateBillingAddress();
  }

  get() {
    this.auth.checkAuthentication();
    this.http.get("http://localhost:5123/api/Addresses/Get/" + this.auth.token?.userId).subscribe({
      next: (res: any) => {
        this.shippingAddress = res;
        console.log(this.shippingAddress);
        this.addressId = this.shippingAddress.id;
        this.contactName = this.shippingAddress.contactName;
        this.country = this.shippingAddress.country;
        this.city = this.shippingAddress.city;
        this.zipCode = this.shippingAddress.zipCode;
        this.description = this.shippingAddress.description;
      },
    });
  }

  create() {
    this.auth.checkAuthentication();
    // shippingRequestAdd nesnesini doldur
    this.shippingRequestAdd.userId = this.auth.token!.userId;
    this.shippingRequestAdd.contactName = this.contactName;
    this.shippingRequestAdd.country = this.country;
    this.shippingRequestAdd.city = this.city;
    this.shippingRequestAdd.zipCode = this.zipCode;
    this.shippingRequestAdd.description = this.description;

    // API isteğini yap
    this.http.post("http://localhost:5123/api/Addresses/Create", this.shippingRequestAdd).subscribe({
      next: (res: any) => {
        this.shippingAddress = res;
        this.swal.callToast("Başarılı bir şekilde adres oluşturdunuz", 'success');

        setTimeout(() => {
          location.reload();
        }, 3000);
      },
      error: (err) => {
        console.error('Error creating address:', err);
      }
    });
  }

  update() {
    this.auth.checkAuthentication();

    // shippingRequestUpdate nesnesini doldur
    this.shippingRequestUpdate.id = this.addressId;
    this.shippingRequestUpdate.userId = this.auth.token!.userId;
    this.shippingRequestUpdate.contactName = this.contactName;
    this.shippingRequestUpdate.country = this.country;
    this.shippingRequestUpdate.city = this.city;
    this.shippingRequestUpdate.zipCode = this.zipCode;
    this.shippingRequestUpdate.description = this.description;

    // API isteğini yap
    this.http.put("http://localhost:5123/api/Addresses/Update", this.shippingRequestUpdate).subscribe({
      next: (res: any) => {
        this.shippingAddress = res;
        this.translate.get("Shipping address updated").subscribe(
          (res: any) => {
            this.swal.callToast(res, 'success');
          }
        );
        setTimeout(() => {
          location.reload();
        }, 3000);
      },
      error: (err) => {
        console.error('Error updating address:', err);
      }
    });
  }

  getBillingAddress() {
    this.auth.checkAuthentication();
    this.http.get("http://localhost:5123/api/Addresses/GetBillingAddress/" + this.auth.token?.userId).subscribe({
      next: (res: any) => {
        this.billingAddress = res;
  
        // Null kontrolü yaparak değerleri atama
        this.billingAddressId = this.billingAddress?.id ?? '';
        this.billingContactName = this.billingAddress?.contactName ?? '';
        this.billingCountry = this.billingAddress?.country ?? '';
        this.billingCity = this.billingAddress?.city ?? '';
        this.billingZipCode = this.billingAddress?.zipCode ?? '';
        this.billingDescription = this.billingAddress?.description ?? '';
      },
      error: (err) => {
        console.error('Error fetching billing address:', err);
        this.swal.callToast("Hata: Fatura adresi alınamadı", 'error');
      }
    });
  }
  
  createBillingAddress() {
    this.auth.checkAuthentication();
  
    
  
    // billingRequestAdd nesnesini doldur
    this.billingRequestAdd.userId = this.auth.token!.userId;
    this.billingRequestAdd.contactName = this.billingContactName;
    this.billingRequestAdd.country = this.billingCountry;
    this.billingRequestAdd.city = this.billingCity;
    this.billingRequestAdd.zipCode = this.billingZipCode;
    this.billingRequestAdd.description = this.billingDescription;
  
    this.http.post("http://localhost:5123/api/Addresses/CreateBillingAddress", this.billingRequestAdd).subscribe({
      next: (res: any) => {
        this.billingAddress = res;
        this.translate.get("Billing address added").subscribe(
          (res: any) => {
            this.swal.callToast(res, 'success');
          }
        );
        setTimeout(() => {
          location.reload();
        }, 3000);
      },
      error: (err) => {
        console.error('Error creating billing address:', err);
        this.swal.callToast("Hata: Fatura adresi oluşturulamadı", 'error');
      }
    });
  }
  
  updateBillingAddress() {
    this.auth.checkAuthentication();
  
    this.billingRequestUpdate.id = this.billingAddressId;
    this.billingRequestUpdate.userId = this.auth.token!.userId;
    this.billingRequestUpdate.contactName = this.billingContactName;
    this.billingRequestUpdate.country = this.billingCountry;
    this.billingRequestUpdate.city = this.billingCity;
    this.billingRequestUpdate.zipCode = this.billingZipCode;
    this.billingRequestUpdate.description = this.billingDescription;
  
    this.http.put("http://localhost:5123/api/Addresses/UpdateBillingAddress", this.billingRequestUpdate).subscribe({
      next: (res: any) => {
        this.billingAddress = res;
        this.translate.get("Billing address updated").subscribe(
          (res: any) => {
            this.swal.callToast(res, 'success');
          }
        );
        setTimeout(() => {
          location.reload();
        }, 3000);
      },
      error: (err) => {
        console.error('Error updating billing address:', err);
        this.swal.callToast("Hata: Fatura adresi güncellenemedi", 'error');
      }
    });
  }
  
}
