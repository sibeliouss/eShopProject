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
        return;
      }
  
      const responseJson = JSON.parse(responseString);
      this.tokenString = responseJson?.accessToken;
      if (!this.tokenString) {
        return;
      }
  
      const decode: any= jwtDecode(this.tokenString);
      this.token = {
        email: decode?.Email,
        name: decode?.Name,
        userName: decode?.UserName,
        userId: decode?.UserId,
        exp: decode?.exp,
        roles: decode?.Roles
      };
  
      console.log(this.token);
  
      const now = new Date().getTime() / 1000;
      if (this.token.exp < now) {
        return this.toLogin();
      }
  
      return true;
    }
    toLogin() {
      this.router.navigateByUrl("/login");
      return false;
    }

 
}
