import { CommonModule } from '@angular/common';

import { RouterLink, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { WishListService } from '../../services/wish-list.service';
import { Component } from '@angular/core';


@Component({
  standalone: true,
  imports: [TranslateModule, CommonModule, RouterModule],
  templateUrl: './wish-list.component.html',
  styleUrls: ['./wish-list.component.scss']
})
export class WishListComponent {
  constructor(public wishList: WishListService) {}
}
