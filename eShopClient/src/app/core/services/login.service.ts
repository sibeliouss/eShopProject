
import { Injectable } from '@angular/core';
import { LoginModel } from '../models/login';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
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
  
  constructor(private http: HttpClient) { }

  post(api: string, data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/${api}`, data).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error("Login request failed:", error);
        return throwError(() => new Error("Login request failed"));
      })
    );
  }
}
