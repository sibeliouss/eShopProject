import { Injectable } from '@angular/core';
import { TokenModel } from '../models/token';
import { UserModel } from '../models/user';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  token: TokenModel | null=null;
  tokenString: string = "";
  user: UserModel = new UserModel();

  firstName: string = "";
  lastName: string = ""; 
  userName: string = "";
  email: string = "";

  constructor(private router: Router, private http: HttpClient, private toast: ToastrService) { }

  checkAuthentication() {
    const responseString = localStorage.getItem("response");
    if (responseString) {
      const responseJson = JSON.parse(responseString);
      this.tokenString = responseJson?.accessToken;
      const decode: any = jwtDecode(this.tokenString);

      // Token kontrolü null ise burada başlatılıyor
      this.token = {
        email: decode?.Email || '',
        name: decode?.Name || '',
        userName: decode?.UserName || '',
        userId: decode?.UserId || '',
        exp: decode?.exp || 0,
        roles: decode?.Roles || []  
      };

      const now = new Date().getTime() / 1000;
      if (this.token.exp < now) {
        return false;
      }
      return true;
    } else {
      this.router.navigateByUrl("/");
      return true;
    }
}

  
  getUser(){
    this.http.get("http://localhost:5123/api/Auth/GetUser/" + this.token?.userId).subscribe({
      next: (res: any) => {
        this.user = res;
        this.firstName = this.user.firstName;
        this.lastName = this.user.lastName;
        this.userName = this.user.userName;
        this.email = this.user.email;
      },
      error: (err: HttpErrorResponse) => {
       
        console.error("Kullanıcı bilgileri alınırken bir hata oluştu:", err.message);
        this.toast.error('Kullanıcı bilgileri alınırken bir hata oluştu.', 'Hata');
      }
    })
  }
  
  logout(): void {
    localStorage.removeItem('response');
    this.router.navigateByUrl('/login');
    this.toast.warning('Çıkış yaptınız.');
   
  }
}