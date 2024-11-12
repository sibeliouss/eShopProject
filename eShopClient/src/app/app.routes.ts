import { Routes } from '@angular/router';
import { LoginComponent } from './core/pages/login/login.component';
import { HomeComponent } from './shared/home/home.component';
import { RegisterComponent } from './core/pages/register/register.component';
import { AccountComponent } from './features/pages/account/account.component';
import { DashboardComponent } from './features/pages/account/dashboard/dashboard.component';
import { AccountDetailsComponent } from './features/pages/account/account-details/account-details.component';
import { AddressesComponent } from './features/pages/account/addresses/addresses.component';
import { OrdersComponent } from './features/pages/account/orders/orders.component';

import { SingleproductComponent } from './features/pages/singleproduct/singleproduct.component';
import { WishListComponent } from './features/pages/wish-list/wish-list.component';
import { WishListeComponent } from './features/pages/account/wish-liste/wish-liste.component';
import { CategoryComponent } from './features/pages/category/category.component';
import { ProductListByCategoryComponent } from './features/pages/product-list-by-category/product-list-by-category.component';
import { SearchComponent } from './features/pages/search/search.component';
import { CartComponent } from './features/pages/cart/cart.component';



export const routes: Routes = [
   
    { path: '', redirectTo: 'homepage', pathMatch: 'full' },
    { path: 'homepage', component: HomeComponent},
    {path:'login',component: LoginComponent},
    { path: 'register', component: RegisterComponent },
    {path:'account/:id', component:AccountComponent,children:[
        { path: '', component: DashboardComponent },
        {path:"account-details", component: AccountDetailsComponent},
        {path:"address", component: AddressesComponent},
        {path:"orders", component: OrdersComponent},
        {path:"wishlist", component:WishListeComponent}
    ]},
    
    {path: "single-product/:value",component: SingleproductComponent},
    {path: "wish-list", component: WishListComponent},
    {path: "categories", component: CategoryComponent},
    {path: 'shop-list/:id',component: ProductListByCategoryComponent},
    {path: "search", component: SearchComponent},
    {path: "cart", component: CartComponent},
   
   
   
   

];
