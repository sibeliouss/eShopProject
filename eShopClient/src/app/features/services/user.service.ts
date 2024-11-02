import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UpdateCustomerInformationModel } from '../models/UpdateCustomerInfomationModel';
import { Observable } from 'rxjs';
import { UpdateUserPasswordModel } from '../models/UpdateCustomerPasswordModel';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:7120/api/Users';

  constructor(private http: HttpClient) {}

  updateUserInformation(updateUserInfo: UpdateCustomerInformationModel): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/UpdateUserInformation`, updateUserInfo);
  }

  updateUserPassword(updatePasswordModel: UpdateUserPasswordModel): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/UpdateUserPassword`, updatePasswordModel);
  }
}
