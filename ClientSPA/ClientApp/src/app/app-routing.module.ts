import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthGuardService } from './services/auth/auth-guard.service';
import { ProductsListComponent } from './products/products-list/products-list.component';

const routes: Routes = [
   { path: '', redirectTo: '/home', pathMatch: 'full' }, // canActivate: [AuthGuardService] redirectTo: '/home',
   { path: 'login', component: LoginComponent },
   { path: 'products', component: ProductsListComponent },
   { path: 'home', component: HomeComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
