import { Routes } from '@angular/router';

import { TestcartComponent } from './carts/testcart/testcart.component';
import { CartComponent } from './carts/cart/cart.component';
import { LoginComponent } from './auth/login/login.component';
import { DashboardComponent } from './auth/dashboard/dashboard.component';
import { AuthGuard } from './auth/auth.guard';
export const routes: Routes = [
  {
    path: 'cart',
    component: CartComponent,
  },
  {
    path: 'home',
    component: TestcartComponent,
  },
  { path: 'login', component: LoginComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '/login' },
];
