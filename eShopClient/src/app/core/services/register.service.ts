import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { SwalService } from './swal.service';
import { RegisterModel } from '../models/register';
import { Observable, catchError, throwError } from 'rxjs';
import { RegisterResponse } from '../models/registerResponse';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  private apiUrl: string = "https://localhost:7120/api";
  

  constructor( private http: HttpClient ) { }

    
  Register(api: string, data: RegisterModel): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/${api}`, data).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error("Request failed:", error);
        return throwError(() => new Error("Request failed"));
      })
    );
  }

}
