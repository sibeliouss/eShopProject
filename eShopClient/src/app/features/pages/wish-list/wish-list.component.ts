import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { WishListService } from '../../services/wish-list.service';


@Component({
  selector: 'app-wish-list',
  standalone: true,
  imports: [TranslateModule, RouterLink, CommonModule],
  templateUrl: './wish-list.component.html',
  styleUrl: './wish-list.component.scss'
})
export class WishListComponent {

  constructor(
    public wishList: WishListService) { }
}
