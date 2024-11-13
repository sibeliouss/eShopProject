import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { PaymentModel } from '../../models/payment';
import { AddressModel } from '../../models/address';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { AuthService } from '../../../core/services/auth.service';
import { AddressService } from '../../services/address.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Countries } from '../../constants/countries';
import { Cities } from '../../constants/cities';
import { Months } from '../../constants/months';
import { Years } from '../../constants/years';


@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [TranslateModule, CommonModule, FormsModule],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent {

 
}
