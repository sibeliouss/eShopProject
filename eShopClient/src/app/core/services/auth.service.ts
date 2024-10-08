import { Injectable } from '@angular/core';
import { TokenModel } from '../models/token';
import { UserModel } from '../models/user';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  
 
  user: UserModel = new UserModel();

  firstName: string = "";
  lastName: string = ""; 
  userName: string = "";
  email: string = "";

  constructor(private router: Router,
    private http: HttpClient) { }

    token: TokenModel | null = null;
    tokenString!: string;
  
  
    checkAuthentication() {
      const responseString = localStorage.getItem("response");
      if (!responseString) {
        return false; // Yanlış durumda false döndür
      }
    
      const responseJson = JSON.parse(responseString);
      this.tokenString = responseJson?.accessToken;
      if (!this.tokenString) {
        return false; // Yanlış durumda false döndür
      }
    
      const decode: any = jwtDecode(this.tokenString);
      this.token = {
        email: decode?.Email,
        name: decode?.Name,
        userName: decode?.UserName,
        userId: decode?.UserId,
        exp: decode?.exp,
        roles: decode?.Roles
      };
    
      console.log(this.token.userName);
    
      const now = new Date().getTime() / 1000;
      if (this.token.exp < now) {
        this.toLogin(); // Token süresi dolmuşsa giriş sayfasına yönlendirin
        return false; // Yanlış durumda false döndür
      }
    
      return true; // Token geçerli
    }
    toLogin() {
      this.router.navigateByUrl("/login");
      return false;
    }

    getUser() {
      if (!this.token || !this.token.userId) {
        console.error("Token or UserId is null."); // Hata mesajı
        return; // Token veya userId null ise çık
      }
    
      const headers = {
        "Authorization": "Bearer " + this.tokenString
      };
    
      this.http.get(`http://localhost:5123/api/Auth/GetUserById/${this.token.userId}`, { headers }).subscribe({
        next: (res: any) => {
          this.user = res; // Kullanıcıyı ayarlayın
          this.firstName = this.user.firstName;
          this.lastName = this.user.lastName;
          this.userName = this.user.userName;
          this.email = this.user.email;
          console.log("User data:", this.user); // Kullanıcı verilerini konsola yazdır
        },
        error: (err) => {
          console.error("Error fetching user data:", err);
        }
      });
    }
 
}