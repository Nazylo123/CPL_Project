import { Component } from '@angular/core';
import { Routes } from '@angular/router';

import { TestcartComponent } from './carts/testcart/testcart.component';
import { CartComponent } from './carts/cart/cart.component';
import { LoginComponent } from './auth/login/login.component';
import { DashboardComponent } from './auth/dashboard/dashboard.component';
import { AuthGuard } from './auth/auth.guard';
import path from 'path';
import { RegisterComponent } from './auth/register/register.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { UpdateUserComponent } from './users/update-user/update-user.component';
import { GetByEmailComponent } from './users/get-by-email/get-by-email.component';
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
    path: 'register',
    component: RegisterComponent,
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
  },

  { path: 'user_list', component: UserListComponent },
  {
    path: 'user_update',
    component: UpdateUserComponent,
  },
  { path: 'get_by_email', component: GetByEmailComponent },
  { path: '**', redirectTo: '/home' },
];
