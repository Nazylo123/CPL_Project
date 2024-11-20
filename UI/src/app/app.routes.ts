import { Routes } from '@angular/router';
import { CartComponent } from './carts/cart/cart.component';
import { TestcartComponent } from './carts/testcart/testcart.component';

export const routes: Routes = [
  {
    path: 'cart',
    component: CartComponent,
  },
  {
    path: 'home',
    component: TestcartComponent,
  },
];
