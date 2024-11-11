import { Injectable } from '@angular/core';
import { TokenModel } from '../models/token';
import { UserModel } from '../models/user';
import { ActivatedRoute, Router } from '@angular/router';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { jwtDecode } from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  token: TokenModel | null = null;
  tokenString!: string;
  user: UserModel = new UserModel();
 

  firstName!: string;
  lastName!: string; 
  userName!: string;
  email!: string;
 

  constructor(
    private router: Router, 
    private http: HttpClient, 
   
  ) {}

  checkAuthentication(): boolean {
    const responseString = localStorage.getItem("response");
    if (responseString) {
      const responseJson = JSON.parse(responseString);
      this.tokenString = responseJson?.accessToken;
      const decode: any = jwtDecode(this.tokenString);

      this.token = {
        email: decode?.Email,
        name: decode?.Name,
        userName: decode?.UserName,
        userId: decode?.UserId,
        exp: decode?.exp,
        roles: decode?.Roles  
      };

      const now = new Date().getTime() / 1000;
      if (this.token.exp < now) {
        return false;
      }
      return true;
    } else {
      this.router.navigateByUrl("/");
      return false;
    }
  }

  getUser(): void {
    this.http.get(`https://localhost:7120/api/Auth/GetUser/${this.token?.userId}`).subscribe({
      next: (res: any) => {
        this.user = res;
        this.firstName = this.user.firstName;
        this.lastName = this.user.lastName;
        this.userName = this.user.userName;
        this.email = this.user.email;
      },
      error: (err: HttpErrorResponse) => {
        console.error("Kullanıcı bilgileri alınırken bir hata oluştu:", err.message);
      }
    });
  }
  logout(): void {
    localStorage.removeItem('response');
    this.router.navigateByUrl('/login');
  }
}
