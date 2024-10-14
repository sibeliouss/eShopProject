import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { LoginService } from '../../services/login.service';
import { BaseInputErrorsComponent } from '../../components/base-input-errors/base-input-errors.component';
import { SwalService } from '../../services/swal.service';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ TranslateModule, FormsModule, ReactiveFormsModule, RouterModule, BaseInputErrorsComponent,CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  
  loginForm!: FormGroup;
  passwordLoginHidden = true;

  constructor(
    private router: Router,
    public login: LoginService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    public authService: AuthService
  ) { }


   ngOnInit(): void {
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = this.formBuilder.group({
      userNameOrEmail: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [true],
    });
  }

 

  signIn() {
    if (this.loginForm.invalid) {
      return;
    }
  
    const request = this.loginForm.value;
    console.log("Kullanıcı Adı:", request.userNameOrEmail);
    
    this.login.post('Auth/Login', request).subscribe({
      next: (res: any) => {
        console.log("Response from server:", res);
        // Token'ı yerel depolamaya kaydedin
        localStorage.setItem('response', JSON.stringify(res));
         this.authService.checkAuthentication();
        this.toastr.success('Giriş başarılı');
        this.router.navigateByUrl("/homepage");
      },
      error: (err: any) => {
        this.toastr.error('Giriş işlemi başarısız oldu.', 'Hata');
      }
    });
  }
  
  SignInPasswordVisibility() {
    this.passwordLoginHidden = !this.passwordLoginHidden;
  }
}