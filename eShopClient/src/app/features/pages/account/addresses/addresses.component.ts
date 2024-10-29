import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { AddressService } from '../../../services/address.service';
import { AddAddressModel } from '../../../models/addAddress';
import { UpdateAddressModel } from '../../../models/updateAddress';
import { AddressModel } from '../../../models/address';
import { SwalService } from '../../../../core/services/swal.service';
import { Countries } from '../../../constants/countries';
import { Cities } from '../../../constants/cities';
import { AuthService } from '../../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-addresses',
  standalone: true,
  imports: [TranslateModule, FormsModule, CommonModule],
  templateUrl: './addresses.component.html',
  styleUrl: './addresses.component.scss'
})
export class AddressesComponent 
{

  shippingRequestAdd: AddAddressModel = { userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };
  shippingRequestUpdate: UpdateAddressModel = { id: '', userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };
  billingRequestAdd: AddAddressModel = { userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };
  billingRequestUpdate: UpdateAddressModel = { id: '', userId: '', contactName: '', country: '', city: '', zipCode: '', description: '' };

  shippingAddress!: AddressModel;
  billingAddress!: AddressModel;

  isaddShippingAddress: boolean = false;
  isaddBillingAddress: boolean = false;
  isShippingAddress: boolean = false;
  isBillingAddress: boolean = false;

  countries = Countries;
  cities = Cities;


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
    private addressService: AddressService,
    private swal: SwalService,
    private translate: TranslateService,
    private auth: AuthService,
    private toast: ToastrService
  ) {}

  ngOnInit(): void {
    this.getShippingAddress();
    this.getBillingAddress();
  }

  getShippingAddress() {
    this.addressService.get().subscribe({
      next: (res: any) => {
        this.shippingAddress = res;
        this.shippingFields();
      },
      error: (err) => {
        console.error('Error fetching shipping address:', err);
      }
    });
  }

  createShippingAddress() {
    this.auth.checkAuthentication();
    
    this.shippingRequestAdd.userId = this.auth.token!.userId;
    this.shippingRequestAdd.contactName = this.contactName;
    this.shippingRequestAdd.country = this.country;
    this.shippingRequestAdd.city = this.city;
    this.shippingRequestAdd.zipCode = this.zipCode;
    this.shippingRequestAdd.description = this.description;

    
    this.addressService.createAddress(this.shippingRequestAdd).subscribe({
      next: (res: any) => {
        this.shippingAddress = res;
        this.translate.get("Shipping address added").subscribe(
          (translated: any) => {
            this.swal.callToast(translated, 'success');
          }
        );
        setTimeout(() => {
          location.reload();
        }, 2000);
      },
      error: (err) => {
        console.error('Adres oluşturulurken hata oluştu:', err);
      }
    });
  }
   
  updateShippingAddress() {

    this.shippingRequestUpdate.id = this.addressId;
    this.shippingRequestUpdate.userId = this.auth.token!.userId;
    this.shippingRequestUpdate.contactName = this.contactName;
    this.shippingRequestUpdate.country = this.country;
    this.shippingRequestUpdate.city = this.city;
    this.shippingRequestUpdate.zipCode = this.zipCode;
    this.shippingRequestUpdate.description = this.description;

    this.addressService.update(this.shippingRequestUpdate).subscribe({
      next: (res: any) => {
        this.shippingAddress = res;
        this.translate.get("Shipping address updated").subscribe(
          (translated: any) => {
            this.swal.callToast(translated, 'success');
          }
        );
        setTimeout(() => {
          location.reload();
        }, 2000);
      },
      error: (err) => {
        console.error('Error updating address:', err);
      }
    });
  }

  deleteAddress(addressId: string) {
    this.translate.get('deleteConfirmation').subscribe((confirmationMessage: string) => {
      this.swal.callSwal(confirmationMessage, 'İptal', 'Sil', () => {
        this.addressService.delete(addressId).subscribe({
          next: (res: any) => {
            this.translate.get('Address successfully deleted').subscribe((successMessage: string) => {
              this.swal.callToast(successMessage, 'success');
            });
            setTimeout(() => {
              location.reload();
            }, 2000);
          },
          error: (err) => {
            console.error('Error deleting address:', err);
          }
        });
      });
    });
  }
  

  getBillingAddress() {
    this.addressService.getBillingAddress().subscribe({
      next: (res: any) => {
        this.billingAddress = res;
        this.billingFields();
      },
      error: (err) => {
        console.error('Error fetching billing address:', err);
      
      }
    });
  }

  createBillingAddress() {

    this.billingRequestAdd.userId = this.auth.token!.userId;
    this.billingRequestAdd.contactName = this.billingContactName;
    this.billingRequestAdd.country = this.billingCountry;
    this.billingRequestAdd.city = this.billingCity;
    this.billingRequestAdd.zipCode = this.billingZipCode;
    this.billingRequestAdd.description = this.billingDescription;


    this.addressService.createBillingAddress(this.billingRequestAdd).subscribe({
      next: (res: any) => {
        this.billingAddress = res;
        this.translate.get("Billing address added").subscribe(
          (translated: any) => {
            this.swal.callToast(translated, 'success');
          }
        );
        setTimeout(() => {
          location.reload();
        }, 2000);
      },
      error: (err) => {
        console.error('Error creating billing address:', err);
        this.swal.callToast("Hata: Fatura adresi oluşturulamadı", 'error');
      }
    });
  }

  updateBillingAddress() {

    this.billingRequestUpdate.id = this.billingAddressId;
    this.billingRequestUpdate.userId = this.auth.token!.userId;
    this.billingRequestUpdate.contactName = this.billingContactName;
    this.billingRequestUpdate.country = this.billingCountry;
    this.billingRequestUpdate.city = this.billingCity;
    this.billingRequestUpdate.zipCode = this.billingZipCode;
    this.billingRequestUpdate.description = this.billingDescription;
    
    this.addressService.updateBillingAddress(this.billingRequestUpdate).subscribe({
      next: (res: any) => {
        this.billingAddress = res;
        this.translate.get("Billing address updated").subscribe(
          (translated: any) => {
            this.swal.callToast(translated, 'success');
          }
        );
        setTimeout(() => {
          location.reload();
        }, 2000);
      },
      error: (err) => {
        console.error('Error updating billing address:', err);
      
      }
    });
  }

  deleteBillingAddress(addressId: string) {
    this.translate.get('deleteConfirmation').subscribe((confirmationMessage: string) => {
      this.swal.callSwal(confirmationMessage, 'Cancel', 'Delete', () => {
        this.addressService.deleteBillingAddress(addressId).subscribe({
          next: (res: any) => {
            this.translate.get('Address successfully deleted').subscribe((successMessage: string) => {
              this.toast.success(successMessage);
            });
            setTimeout(() => {
              location.reload();
            }, 2000);
          },
          error: (err) => {
            console.error('Error deleting address:', err);
          }
        });
      });
    });
  }
  

  shippingFields() {
    this.addressId=this.shippingAddress.id;
    this.contactName = this.shippingAddress.contactName;
    this.country = this.shippingAddress.country;
    this.city = this.shippingAddress.city;
    this.zipCode = this.shippingAddress.zipCode;
    this.description = this.shippingAddress.description;
  }

  billingFields() {
    this.billingAddressId = this.billingAddress?.id ?? '';
        this.billingContactName = this.billingAddress?.contactName ?? '';
        this.billingCountry = this.billingAddress?.country ?? '';
        this.billingCity = this.billingAddress?.city ?? '';
        this.billingZipCode = this.billingAddress?.zipCode ?? '';
        this.billingDescription = this.billingAddress?.description ?? '';
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
    this.createShippingAddress();
  }

  addBillingButton() {
    this.editBillingAddress();
    this.createBillingAddress();
  }

  updateButton() {
    this.editShippingAddress();
    this.updateShippingAddress();
  }

  updateBillingButton() {
    this.editBillingAddress();
    this.updateBillingAddress();
  }
}

