import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddAddressModel } from '../models/addAddress';
import { UpdateAddressModel } from '../models/updateAddress';
import { AuthService } from '../../core/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  constructor(
    private http: HttpClient,
    private auth: AuthService
  ) { }

  get(): Observable<any> {
    this.auth.checkAuthentication();
    return this.http.get("https://localhost:7120/api/Addresses/Get/" + this.auth.token?.userId);
  }

  createAddress(shippingRequestAdd: AddAddressModel): Observable<any> {
    return this.http.post("https://localhost:7120/api/Addresses/Create", shippingRequestAdd);
  }

  update(shippingRequestUpdate: UpdateAddressModel): Observable<any> {
    this.auth.checkAuthentication();
    return this.http.put("https://localhost:7120/api/Addresses/Update", shippingRequestUpdate);
  }
  
  delete(addressId: string): Observable<any> {
    this.auth.checkAuthentication();
    return this.http.delete("https://localhost:7120/api/Addresses/Delete/" + addressId);
  }
  
  getBillingAddress(): Observable<any> {
    this.auth.checkAuthentication();
    return this.http.get("https://localhost:7120/api/Addresses/GetBillingAddress/" + this.auth.token?.userId);
  }

  createBillingAddress(billingRequestAdd: AddAddressModel): Observable<any> {
    this.auth.checkAuthentication();
    return this.http.post("https://localhost:7120/api/Addresses/CreateBillingAddress", billingRequestAdd);
  }

  updateBillingAddress(billingRequestUpdate: UpdateAddressModel): Observable<any> {
    this.auth.checkAuthentication();
    return this.http.put("https://localhost:7120/api/Addresses/UpdateBillingAddress", billingRequestUpdate);
  }
  deleteBillingAddress(addressId: string): Observable<any> {
    this.auth.checkAuthentication();
    return this.http.delete("https://localhost:7120/api/Addresses/DeleteBillingAddress/" + addressId);
  }
  
}
