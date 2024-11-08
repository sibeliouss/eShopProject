import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { WishListService } from '../../../services/wish-list.service';

@Component({
  selector: 'app-wish-liste',
  standalone: true,
  imports: [TranslateModule, CommonModule, RouterLink],
  templateUrl: './wish-liste.component.html',
  styleUrl: './wish-liste.component.scss'
})
export class WishListeComponent {

  constructor(
    public wishlist: WishListService
  ){}

}
