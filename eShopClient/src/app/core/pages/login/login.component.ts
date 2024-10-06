import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { LoginService } from '../../services/login.service';
import { BaseInputErrorsComponent } from '../../components/base-input-errors/base-input-errors.component';
import { SwalService } from '../../services/swal.service';
import { AuthService } from '../../services/auth.service';

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
  responseInLocalStorage: any;

  constructor(
    private router: Router,
    public login: LoginService,
    private formBuilder: FormBuilder,
    private swal : SwalService,
    private authService: AuthService
  
  ) {
    this.loginForm = this.formBuilder.group({
      userNameOrEmail: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [true],
    })
  }


  ngOnInit() {
    if (this.authService.checkAuthentication()) {
      this.router.navigateByUrl('/');
    }
    
  }

  signIn() {

    if (this.loginForm.invalid) {
      this.swal.callToast('Lütfen tüm alanları doldurun.', 'warning'); // Uyarı mesajı
      return;
    }
    const request = this.loginForm.value;

    this.login.post('Auth/Login', request).subscribe({
      next: (res: any) => {
        localStorage.setItem('response', JSON.stringify(res));
        this.authService.checkAuthentication();
        this.router.navigateByUrl('/');
      },
      error: (err: any) => {
        this.swal.callToast('Giriş işlemi başarısız oldu.', 'error');  // Hata mesajı
      }
    });

    
  }

  SignInPasswordVisibility() {
    this.passwordLoginHidden = !this.passwordLoginHidden;
  }

}
