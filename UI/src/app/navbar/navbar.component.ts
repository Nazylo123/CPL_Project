import { CartServiceService } from './../carts/services/cart.service';
import { CartRequest } from './../carts/models/cart.request';
import { CookieService } from 'ngx-cookie-service';
import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  cartItems: CartRequest[] = [];
  sizeCart: number = 0;
  constructor(
    private cookieService: CookieService,
    private cartService: CartServiceService
  ) {}

  ngOnInit(): void {
    this.cartItems = this.cartService.getCartItems();
    this.sizeCart = this.cartItems.length;
  }
}
