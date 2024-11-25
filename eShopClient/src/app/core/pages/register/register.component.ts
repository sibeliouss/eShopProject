import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { SwalService } from '../../services/swal.service';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BaseInputErrorsComponent } from '../../components/base-input-errors/base-input-errors.component';
import { RegisterService } from '../../services/register.service';
import { ToastrService } from 'ngx-toastr';
import { RegisterModel } from '../../models/register';

@Component({
  standalone: true,
  imports: [RouterModule, TranslateModule, CommonModule, FormsModule, ReactiveFormsModule, BaseInputErrorsComponent],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']

})
export class RegisterComponent {

  signUpForm!: FormGroup;
  passwordSignUpHidden = true;
  confirmPasswordHidden= true;


 constructor( private http: HttpClient,
  private router: Router,
  private translate: TranslateService,
  private toastr: ToastrService,
  private formBuilder: FormBuilder,
  private register: RegisterService){ }

  ngOnInit(): void {
    this.createSignUpForm();
  }

  createSignUpForm() {
    this.signUpForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmedPassword: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  signUp(){
    if (this.signUpForm.invalid) {
      return;
    }
    const password = this.signUpForm.get('password')?.value;
    const confirmedPassword = this.signUpForm.get('confirmedPassword')?.value;
  
    if (password !== confirmedPassword) {
    this.toastr.error('Şifreler uyuşmuyor. Lütfen şifrelerinizi kontrol edin.', 'Hata');
    return;
  }
    const request: RegisterModel = this.signUpForm.value; 
    console.log("Kullanıcı Adı:", request.userName);

    this.register.Register('Auth/Register', request).subscribe({
      next: (res) => {
        console.log("Response from server:", res);
        this.toastr.success('Kayıt başarılı');
        this.router.navigateByUrl("/login");
      },
      error: (err: any) => {
        this.toastr.error('Kayıt işlemi başarısız oldu.', 'Hata');
      }
    });

  }


  SignUpPasswordVisibility() {
    this.passwordSignUpHidden = !this.passwordSignUpHidden;
  }

  ConfirmPasswordVisibility() {
    this.confirmPasswordHidden = !this.confirmPasswordHidden;
  }

}
