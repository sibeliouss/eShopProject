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

  constructor(private translate: TranslateService){
    //varsayılan dil
    this.translate.setDefaultLang('tr');
  }
  
    switchLanguage(language: string) {
      this.translate.use(language);
    }
  

}
