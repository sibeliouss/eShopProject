import { Component } from '@angular/core';
import { AuthService } from '../../../../core/services/auth.service';
import { TranslateModule } from '@ngx-translate/core';
import { Router, RouterModule } from '@angular/router';
import { ShoppingCartService } from '../../../services/shopping-cart.service';

@Component({
 
  standalone: true,
  imports: [TranslateModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

  constructor( public auth: AuthService,  private router: Router, private shopping: ShoppingCartService){}
  logout(): void {
    localStorage.removeItem('response');
    this.shopping.GetAllCarts();
    this.router.navigateByUrl('/login');
  }

}
