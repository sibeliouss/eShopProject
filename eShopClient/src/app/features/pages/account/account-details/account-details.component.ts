import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { UpdateCustomerInformationModel } from '../../../models/UpdateCustomerInfomationModel';
import { UpdateCustomerPasswordModel } from '../../../models/UpdateCustomerPasswordModel';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../../../core/services/auth.service';
import { SwalService } from '../../../../core/services/swal.service';
import { CustomerService } from '../../../services/customer.service';
import { Customer } from '../../../models/Customer';

@Component({
  selector: 'app-account-details',
  standalone: true,
  imports: [TranslateModule, CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent {
  customerId!: string;
  customer: Customer | undefined;
  requestUserInfomation!: UpdateCustomerInformationModel;
  requestUserPassword!: UpdateCustomerPasswordModel;

  accountForm: FormGroup;
  passwordForm: FormGroup;

  constructor(
    private http: HttpClient,
    public auth: AuthService,
    private swal: SwalService,
    public customerService: CustomerService,
    private translate: TranslateService,
  ) {
    this.auth.checkAuthentication();
    this.getCustomerByUserId(this.customerId);

    // Initialize form groups
    this.accountForm = new FormGroup({
      firstName: new FormControl(''),
      lastName: new FormControl(''),
      email: new FormControl(''),
      userName: new FormControl('')
    });

    this.passwordForm = new FormGroup({
      currentPassword: new FormControl(''),
      newPassword: new FormControl(''),
      confirmNewPassword: new FormControl('')
    });
  }
  getCustomerByUserId(id: string): void {
    this.customerService.getCustomerByUserId(id).subscribe(
      (data: Customer) => {
        this.customer = data; // Customer object is populated
        // Update the form with customer data
        this.accountForm.patchValue({
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
          userName: data.userName
        });
      },
      (error) => {
        console.error('Error fetching customer data:', error);
      }
    );
  }

  updateUserInformation() {
    if (this.customer) {
      // Eğer müşteri bilgileri yüklenmişse
      this.requestUserInfomation.id = this.customer.userId; // ID'yi customer objesinden al
      this.requestUserInfomation.firstName = this.accountForm.value.firstName;
      this.requestUserInfomation.lastName = this.accountForm.value.lastName;
      this.requestUserInfomation.userName = this.accountForm.value.userName;
      this.requestUserInfomation.email = this.accountForm.value.email;
  
      this.http.post("https://localhost:5123/api/Auth/UpdateCustomerInformation", this.requestUserInfomation).subscribe({
        next: (res: any) => {
          this.translate.get("Account information updated!").subscribe((message: string) => {
            this.swal.callToast(message, 'success');
          });
          setTimeout(() => {
            location.reload(); // Sayfayı yeniler
          }, 3000);
        },
        error: (error) => {
          console.error('Error updating customer information:', error);
        }
      });
    } else {
      console.error('Customer information not loaded.');
    }
  }
  

  updateUserPassword() {
    this.requestUserPassword.id = this.customerId;
    this.requestUserPassword.currentPassword = this.passwordForm.value.currentPassword;
    this.requestUserPassword.newPassword = this.passwordForm.value.newPassword;
    this.requestUserPassword.confirmedPassword = this.passwordForm.value.confirmNewPassword;

    this.http.post("https://localhost:5123/api/Auth/UpdateCustomerPassword", this.requestUserPassword).subscribe({
      next: (res: any) => {
        this.translate.get("Account password updated!").subscribe((message: string) => {
          this.swal.callToast(message, 'success');
        });
        setTimeout(() => {
          location.reload();
        }, 3000);
      },
      error: (error) => {
        console.error('Error updating customer password:', error);
      }
    });
  }
}
