import { Component } from '@angular/core';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss'
})
export class TopbarComponent {

  constructor(private translate: TranslateService) {
    // Varsayılan dil
    const savedLang = localStorage.getItem('language') || 'tr'; // Yerel depolama kontrolü
    this.translate.setDefaultLang('tr');
    this.translate.use(savedLang); 
  }

  switchLanguage(language: string) {
    this.translate.use(language);
    localStorage.setItem('language', language); 
  }
  

}
