import { Routes } from '@angular/router';

import { TestcartComponent } from './carts/testcart/testcart.component';
import { CartComponent } from './carts/cart/cart.component';

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
