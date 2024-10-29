import { Component } from '@angular/core';
import { TopbarComponent } from "./topbar/topbar.component";
import { MiddlebarComponent } from "./middlebar/middlebar.component";
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FooterComponent } from "./footer/footer.component";

@Component({
  selector: 'app-layouts',
  standalone: true,
  imports: [TopbarComponent, MiddlebarComponent, CommonModule, FooterComponent],
  templateUrl: './layouts.component.html',
  styleUrl: './layouts.component.scss'
})
export class LayoutsComponent {

  showBars: boolean = true;

  constructor(private router: Router) {
   
    this.router.events.subscribe((event: any) => {
      if (event.url) {
        // Hem login hem de register sayfalarında gizlenmesi için kontrol
        this.showBars = !(event.url.includes('/login') || event.url.includes('/register'));
      }
    });
  }

}
