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

@Component({
  selector: 'app-register',
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
  private register: RegisterService){

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
    const request = this.signUpForm.value;
    console.log("Kullanıcı Adı:", request.userName);
    
    this.register.post('Auth/Register', request).subscribe({
      next: (res: any) => {
        console.log("Response from server:", res);
        // Token'ı yerel depolamaya kaydedin
       
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
