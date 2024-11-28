import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { forkJoin } from 'rxjs';
import { BaseInputErrorsComponent } from '../../../../core/components/base-input-errors/base-input-errors.component';


@Component({
  standalone: true,
  imports: [TranslateModule, FormsModule, CommonModule, ReactiveFormsModule, BaseInputErrorsComponent],
  templateUrl: './addresses.component.html',
  styleUrl: './addresses.component.scss'
})
export class AddressesComponent 
{
  shippingAddressForm!: FormGroup;
  billingAddressForm!: FormGroup;

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
    private toast: ToastrService,
    private fb: FormBuilder
  ) {}


  ngOnInit(): void {
    this.getShippingAddress();
    this.getBillingAddress();
  
    // Initialize the forms
    this.shippingAddressForm = this.fb.group({
      contactName: [this.contactName, [Validators.required]],
      description: [this.description, [Validators.required]],
      city: [this.city, [Validators.required]],
      zipCode: [this.zipCode, [Validators.required, Validators.pattern(/^\d{5}$/) ]],
      country: [this.country, [Validators.required]],
    });
  
    this.billingAddressForm = this.fb.group({
      billingContactName: [this.billingContactName, [Validators.required]],
      billingDescription: [this.billingDescription, [Validators.required]],
      billingCity: [this.billingCity, [Validators.required]],
      billingZipCode: [this.billingZipCode, [Validators.required, Validators.pattern(/^\d{5}$/) ]],
      billingCountry: [this.billingCountry, [Validators.required]],
    });
  }
  

  getShippingAddress() {
    this.addressService.get().subscribe({
      next: (res: any) => {
        this.shippingAddress = res;
        this.shippingFields();
  
     
        this.shippingAddressForm.patchValue({
          contactName: this.shippingAddress.contactName,
          description: this.shippingAddress.description,
          city: this.shippingAddress.city,
          zipCode: this.shippingAddress.zipCode,
          country: this.shippingAddress.country,
        });
      },
      error: (err) => {
        console.error('Error fetching shipping address:', err);
      }
    });
  }

  createShippingAddress() {
    this.auth.checkAuthentication();

    this.shippingRequestAdd = {
        userId: this.auth.token!.userId,
        contactName: this.shippingAddressForm.value.contactName,
        country: this.shippingAddressForm.value.country,
        city: this.shippingAddressForm.value.city,
        zipCode: this.shippingAddressForm.value.zipCode,
        description: this.shippingAddressForm.value.description,
    };

    this.addressService.createAddress(this.shippingRequestAdd).subscribe({
        next: (res: any) => {
            this.shippingAddress = res;
            this.translate.get("Adres eklendi").subscribe(
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
  this.shippingRequestUpdate = {
      id: this.addressId,
      userId: this.auth.token!.userId,
      contactName: this.shippingAddressForm.value.contactName,
      country: this.shippingAddressForm.value.country,
      city: this.shippingAddressForm.value.city,
      zipCode: this.shippingAddressForm.value.zipCode,
      description: this.shippingAddressForm.value.description,
  };

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
          console.error('Adres güncellenirken hata oluştu:', err);
      }
  });
}

  deleteAddress(addressId: string): void {
    forkJoin({
      deleteMessage: this.translate.get("remove.doYouWantToDeleted"),
      cancel: this.translate.get("remove.cancelButton"),
      confirm: this.translate.get("remove.confirmButton")
    }).subscribe(translations => {
    
      this.swal.callSwal(translations.deleteMessage, translations.cancel, translations.confirm, () => {
      
        this.addressService.delete(addressId).subscribe({
          next: () => {
          this.translate.get('Address successfully deleted').subscribe(successMessage => {
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
  
        // Formu doldurma
        this.billingAddressForm.patchValue({
          billingContactName: this.billingAddress.contactName,
          billingDescription: this.billingAddress.description,
          billingCity: this.billingAddress.city,
          billingZipCode: this.billingAddress.zipCode,
          billingCountry: this.billingAddress.country,
        });
      },
      error: (err) => {
        console.error('Error fetching billing address:', err);
      }
    });
  }

  createBillingAddress() {
    if (this.billingAddressForm.valid) {
      this.billingRequestAdd.userId = this.auth.token!.userId;
      this.billingRequestAdd.contactName = this.billingAddressForm.value.billingContactName;
      this.billingRequestAdd.country = this.billingAddressForm.value.billingCountry;
      this.billingRequestAdd.city = this.billingAddressForm.value.billingCity;
      this.billingRequestAdd.zipCode = this.billingAddressForm.value.billingZipCode;
      this.billingRequestAdd.description = this.billingAddressForm.value.billingDescription;
  
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
          this.swal.callToast("Error: Billing address could not be created", 'error');
        }
      });
    }
  }
  
  updateBillingAddress() {
    if (this.billingAddressForm.valid) {
      this.billingRequestUpdate.id = this.billingAddressId;
      this.billingRequestUpdate.userId = this.auth.token!.userId;
      this.billingRequestUpdate.contactName = this.billingAddressForm.value.billingContactName;
      this.billingRequestUpdate.country = this.billingAddressForm.value.billingCountry;
      this.billingRequestUpdate.city = this.billingAddressForm.value.billingCity;
      this.billingRequestUpdate.zipCode = this.billingAddressForm.value.billingZipCode;
      this.billingRequestUpdate.description = this.billingAddressForm.value.billingDescription;
  
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
  }
  

  deleteBillingAddress(addressId: string) {
    forkJoin({
      deleteMessage: this.translate.get("remove.doYouWantToDeleted"),
      cancel: this.translate.get("remove.cancelButton"),
      confirm: this.translate.get("remove.confirmButton")
    }).subscribe(translations => {
    
      this.swal.callSwal(translations.deleteMessage, translations.cancel, translations.confirm, () => {
      
        this.addressService.deleteBillingAddress(addressId).subscribe({
          next: () => {
          this.translate.get('Billing address successfully deleted').subscribe(successMessage => {
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
  

  shippingFields() {
    this.addressId=this.shippingAddress.id;
    this.contactName = this.shippingAddress.contactName;
    this.country = this.shippingAddress.country;
    this.city = this.shippingAddress.city;
    this.zipCode = this.shippingAddress.zipCode;
    this.description = this.shippingAddress.description;
  }

  billingFields() {
    this.billingAddressId = this.billingAddress.id;
        this.billingContactName = this.billingAddress.contactName ;
        this.billingCountry = this.billingAddress.country;
        this.billingCity = this.billingAddress.city;
        this.billingZipCode = this.billingAddress.zipCode;
        this.billingDescription = this.billingAddress?.description;
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