import { Routes } from '@angular/router';
import { LoginComponent } from './core/pages/login/login.component';
import { HomeComponent } from './shared/home/home.component';
import { AuthService } from './core/services/auth.service';
import { RegisterComponent } from './core/pages/register/register.component';

export const routes: Routes = [
   
    { path: '', redirectTo: 'homepage', pathMatch: 'full' },
    { path: 'homepage', component: HomeComponent},
    {path:'login',component: LoginComponent},
    { path: 'register', component: RegisterComponent },

];
