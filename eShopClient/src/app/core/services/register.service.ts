import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { SwalService } from './swal.service';
import { RegisterModel } from '../models/register';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  private apiUrl: string = "http://localhost:5123/api";
  

  constructor( private http: HttpClient ) { }

    
  post(api: string, data: RegisterModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/${api}`, data).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error("Request failed:", error);
        return throwError(() => new Error("Request failed"));
      })
    );
  }

}
