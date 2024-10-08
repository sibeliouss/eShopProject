import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserModel } from '../../../core/models/user';

@Component({
  selector: 'app-middlebar',
  standalone: true,
  imports: [RouterModule, TranslateModule,CommonModule, FormsModule],
  templateUrl: './middlebar.component.html',
  styleUrl: './middlebar.component.scss'
})
export class MiddlebarComponent {

  responseInLocalStorage: any;  

  constructor(public auth: AuthService) {}

  ngOnInit() {
    this.responseInLocalStorage = localStorage.getItem("response");
    if (this.auth.checkAuthentication()) {
      this.auth.getUser(); // Kullanıcı verilerini al
    } else {
      console.error("User is not authenticated."); // Giriş yapılmamışsa hata mesajı
    }
  }

  isLoggedIn() {
    return this.auth.checkAuthentication(); // Kullanıcı giriş yapmış mı
  }
}
