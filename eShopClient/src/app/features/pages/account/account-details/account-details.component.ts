import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AuthService } from '../../../../core/services/auth.service';
import { SwalService } from '../../../../core/services/swal.service';
import { Router } from '@angular/router';
import { UpdateCustomerInformationModel } from '../../../models/UpdateCustomerInfomationModel';
import { UpdateUserPasswordModel } from '../../../models/UpdateCustomerPasswordModel';
import { UserService } from '../../../services/user.service';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-account-details',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TranslateModule],
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent implements OnInit {
  accountForm!: FormGroup;
  requestUserPassword: UpdateUserPasswordModel = new UpdateUserPasswordModel();

  currentPassword!: string;
  newPassword!: string;
  confirmedPassword!: string;

  constructor(
    private fb: FormBuilder,
    private userService: UserService, 
    public auth: AuthService,
    private swal: SwalService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForms();
   
  }

  private initializeForms() {
    this.accountForm = this.fb.group({
      firstName: [this.auth.firstName, [Validators.required]],
      lastName: [this.auth.lastName, [Validators.required]],
      email: [this.auth.email, [Validators.required, Validators.email]],
      userName: [this.auth.userName, [Validators.required]]
    });
  }

  updateUserInformation() {
    if (this.accountForm.valid) {
      const { firstName, lastName, email, userName } = this.accountForm.value;

      const updateUserInfo: UpdateCustomerInformationModel = {
        id: this.auth.token!.userId,
        firstName,
        lastName,
        email,
        userName
      };

      this.userService.updateUserInformation(updateUserInfo).subscribe({
        next: (res) => {
          this.swal.callToast(res.message, 'success');
          setTimeout(() => {
            this.router.navigate([`/account/${this.auth.token?.userId}`]).then(() => {
              location.reload(); 
            });
          }, 3000);
        },
        error: (err) => {
          this.swal.callToast("Güncelleme sırasında hata oluştu: " + err.message, 'error');
        }
      });
    }
  }

  updateUserPassword() {
    this.requestUserPassword.id = this.auth.token!.userId;

    this.requestUserPassword.currentPassword = this.currentPassword;
    this.requestUserPassword.newPassword = this.newPassword;
    this.requestUserPassword.confirmedPassword = this.confirmedPassword;

    this.userService.updateUserPassword(this.requestUserPassword).subscribe({
      next: (res) => {
        this.swal.callToast(res.message, 'success');
        setTimeout(() => {
          this.router.navigate([`/account/${this.auth.token?.userId}`]).then(() => {
            location.reload(); 
          });
        }, 3000);
      },
      error: (err) => {
        this.swal.callToast("Güncelleme sırasında hata oluştu: " + err.message, 'error');
      }
    });
  }

  logout() {
    localStorage.removeItem("response");
    location.href = "/login";
  }
}
