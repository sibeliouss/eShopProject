import { Injectable } from '@angular/core';
import { TokenModel } from '../models/token';
import { UserModel } from '../models/user';
import { ActivatedRoute, Router } from '@angular/router';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { Customer } from '../../features/models/Customer';
import { CustomerService } from '../../features/services/customer.service';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  token: TokenModel | null = null;
  tokenString: string = '';
  user: UserModel = new UserModel();
  customer: Customer | null = null;

  firstName: string = '';
  lastName: string = ''; 
  userName: string = '';
  email: string = '';
  customerId: string | null = null;

  constructor(
    private router: Router, 
    private http: HttpClient, 
    private toast: ToastrService,
    private activeRoute: ActivatedRoute, 
    private customerService: CustomerService
  ) {}

  checkAuthentication(): boolean {
    const responseString = localStorage.getItem("response");
    if (responseString) {
      const responseJson = JSON.parse(responseString);
      this.tokenString = responseJson?.accessToken;
      const decode: any = jwtDecode(this.tokenString);

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
      return false;
    }
  }

  getUser(): void {
    this.http.get(`http://localhost:5123/api/Auth/GetUser/${this.token?.userId}`).subscribe({
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

  getCustomer(customerId: string): void {
    this.customerService.getCustomerById(customerId).subscribe({
      next: (response: Customer) => {
        if (response.userId === this.token?.userId) {
          this.customer = response; 
        } else {
          console.error("Kullanıcı ve müşteri bilgileri eşleşmiyor.");
        }
      },
      error: (err: HttpErrorResponse) => {
        console.error("Müşteri bilgileri alınırken bir hata oluştu:", err.message);
      }
    });
  }
  

  logout(): void {
    localStorage.removeItem('response');
    this.router.navigateByUrl('/login');
  }
}
