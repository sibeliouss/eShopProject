import { Component } from '@angular/core';
import { TopbarComponent } from "./topbar/topbar.component";
import { MiddlebarComponent } from "./middlebar/middlebar.component";
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BottombarComponent } from "./bottombar/bottombar.component";

@Component({
  selector: 'app-layouts',
  standalone: true,
  imports: [TopbarComponent, MiddlebarComponent, CommonModule, BottombarComponent],
  templateUrl: './layouts.component.html',
  styleUrl: './layouts.component.scss'
})
export class LayoutsComponent {

  showBars: boolean = true;
  

  constructor(public router: Router) {
   
    this.router.events.subscribe((event: any) => {
      if (event.url) {
        // Hem login hem de register sayfalarında gizlenmesi için kontrol
        this.showBars = !(event.url.includes('/login') || event.url.includes('/register') || event.url.includes('/order-received') );
      }
    });
  }

 
  

}
