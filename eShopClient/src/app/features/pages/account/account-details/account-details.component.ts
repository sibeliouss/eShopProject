import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { UpdateCustomerInformationModel } from '../../../models/UpdateCustomerInfomationModel';
import { UpdateCustomerPasswordModel } from '../../../models/UpdateCustomerPasswordModel';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../../../core/services/auth.service';
import { SwalService } from '../../../../core/services/swal.service';


@Component({
  selector: 'app-account-details',
  standalone: true,
  imports: [TranslateModule, CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './account-details.component.html',
  styleUrls: ['./account-details.component.scss']
})
export class AccountDetailsComponent {

  requestUserInfomation!: UpdateCustomerInformationModel;
  requestUserPassword!: UpdateCustomerPasswordModel;

  
 

  constructor(
    private http: HttpClient,
    public auth: AuthService,
    private swal: SwalService,
    private translate: TranslateService,
  ) {
    
    };
}
