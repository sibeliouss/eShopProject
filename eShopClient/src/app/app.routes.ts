import { Routes } from '@angular/router';
import { LoginComponent } from './core/pages/login/login.component';
import { HomeComponent } from './shared/home/home.component';
import { RegisterComponent } from './core/pages/register/register.component';
import { AccountComponent } from './features/pages/account/account.component';
import { DashboardComponent } from './features/pages/account/dashboard/dashboard.component';
import { AccountDetailsComponent } from './features/pages/account/account-details/account-details.component';
import { AddressesComponent } from './features/pages/account/addresses/addresses.component';
import { OrdersComponent } from './features/pages/account/orders/orders.component';

export const routes: Routes = [
   
    { path: '', redirectTo: 'homepage', pathMatch: 'full' },
    { path: 'homepage', component: HomeComponent},
    {path:'login',component: LoginComponent},
    { path: 'register', component: RegisterComponent },
    {path:'account/:userId', component:AccountComponent,children:[
        { path: '', component: DashboardComponent },
        {path:"account-details", component: AccountDetailsComponent},
        {path:"address", component: AddressesComponent},
        {path:"orders", component: OrdersComponent}
    ]}

];
