import { Injectable } from '@angular/core';
import { LoginModel } from '../models/login';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { SwalService } from './swal.service';
import { Observable, catchError, throwError } from 'rxjs';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private apiUrl: string = "http://localhost:5123/api";
  
  constructor( private http: HttpClient,
    private auth: AuthService,
    private router: Router) { }

    get(api: string): Observable<any> {
      return this.http.get(`${this.apiUrl}/${api}`, {
        headers: {
          "Authorization": "Bearer " + this.auth.tokenString
        }
      });
    }
  
    post(api: string, data: any): Observable<any> {
      return this.http.post(`${this.apiUrl}/${api}`, data, {
        headers: {
          "Authorization": "Bearer " + this.auth.tokenString
        }
      });
    }
    
    
}
