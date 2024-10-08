import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-middlebar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './middlebar.component.html',
  styleUrl: './middlebar.component.scss'
})
export class MiddlebarComponent {

  responseInLocalStorage: any;

  constructor( private router: Router, public authService: AuthService, private toastr:ToastrService){}

 
   
  


}
