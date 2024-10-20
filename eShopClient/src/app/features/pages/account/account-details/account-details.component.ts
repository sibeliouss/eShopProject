import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UpdateCustomerInformationModel } from '../../../models/UpdateCustomerInfomationModel';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../../../core/services/auth.service';
import { SwalService } from '../../../../core/services/swal.service';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { UpdateUserPasswordModel } from '../../../models/UpdateCustomerPasswordModel';

@Component({
  selector: 'app-account-details',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TranslateModule],
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent {
  accountForm!: FormGroup;
  //passwordForm!: FormGroup; // Yeni form grubu ekleniyor
  requestUserPassword: UpdateUserPasswordModel = new UpdateUserPasswordModel();

  currentPassword: string = "";
  newPassword: string = "";
  confirmedPassword: string = "";

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    public auth: AuthService,
    private swal: SwalService,
    private router: Router,
    private translate: TranslateService
  ) {
    this.initializeForms();
    this.auth.checkAuthentication();
    this.auth.getUser();
  }

  private initializeForms() {
    // Formları başlatma
    this.accountForm = this.fb.group({
      firstName: [this.auth.firstName || '', [Validators.required]],
      lastName: [this.auth.lastName || '', [Validators.required]],
      email: [this.auth.email || '', [Validators.required, Validators.email]],
      userName: [this.auth.userName || '', [Validators.required]]
    });

    // Şifre güncelleme formunu başlatma
    /* this.passwordForm = this.fb.group({
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required]],
      confirmedPassword: ['', [Validators.required]]
    }); */
  }

  updateUserInformation() {
    if (this.accountForm.valid) {
      const { firstName, lastName, email, userName } = this.accountForm.value;

      const updateUserInfo: UpdateCustomerInformationModel = {
        id: this.auth.token?.userId || '',
        firstName,
        lastName,
        email,
        userName
      };

      this.http.put<{ message: string }>("http://localhost:5123/api/Users/UpdateUserInformation/", updateUserInfo).subscribe({
        next: (res) => {
          this.swal.callToast(res.message, 'success');
          setTimeout(() => {
            this.router.navigate([`/account/${this.auth.token?.userId}`]).then(() => {
              location.reload(); 
            });
          }, 3000);
        },
        error: (err) => {
          this.swal.callToast("Güncelleme sırasında hata oluştu: " + err.message, 'error'); // Hata mesajı
        }
      });
    }
  }
  updateUserPassword(){
    this.requestUserPassword.id = this.auth.token?.userId ?? '';

    this.requestUserPassword.currentPassword = this.currentPassword;
    this.requestUserPassword.newPassword = this.newPassword;
    this.requestUserPassword.confirmedPassword = this.confirmedPassword;

    this.http.post<{ message: string }>("http://localhost:5123/api/Users/UpdateUserPassword", this.requestUserPassword).subscribe({
      next: (res) => {
        this.swal.callToast(res.message, 'success');
        setTimeout(() => {
          this.router.navigate([`/account/${this.auth.token?.userId}`]).then(() => {
            location.reload(); 
          });
        }, 3000);
      },
      error: (err) => {
        this.swal.callToast("Güncelleme sırasında hata oluştu: " + err.message, 'error'); // Hata mesajı
      }
    });
  }


  // Kullanıcıyı çıkış yap
  logout() {
    localStorage.removeItem("response");
    location.href = "/login";
  }
}
