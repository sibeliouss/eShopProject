import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateModule } from '@ngx-translate/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-middlebar',
  standalone: true,
  imports: [RouterModule, TranslateModule,CommonModule, FormsModule],
  templateUrl: './middlebar.component.html',
  styleUrl: './middlebar.component.scss'
})
export class MiddlebarComponent {

  responseInLocalStorage: any;  


  ngOnInit() {
    if (localStorage.getItem('response')) {
      this.responseInLocalStorage = localStorage.getItem("response");
      this.auth.checkAuthentication();
    }
    this.auth.getUser();
    console.log(this.auth.token?.userId);
    
  }



  constructor(public auth: AuthService, private router: Router) {}
  


  
}
