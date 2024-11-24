import { Routes } from '@angular/router';

import { CartComponent } from './carts/cart/cart.component';
import { HomeComponent } from './users/home/home.component';
import { AboutComponent } from './users/about/about.component';
import { ShopComponent } from './users/shop/shop.component';
import { ShopSingleComponent } from './users/shop-single/shop-single.component';

export const routes: Routes = [
  {
    path: 'cart',
    component: CartComponent,
  },

  {
    path: 'shop',
    component: ShopComponent,
  },
  {
    path: 'about',
    component: AboutComponent,
  },
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'shop-single',
    component: ShopSingleComponent,
  },
];
