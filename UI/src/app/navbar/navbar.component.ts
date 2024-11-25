import { CartRequest } from './../carts/models/cart.request';
import { CookieService } from 'ngx-cookie-service';
import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CartServiceService } from '../carts/services/cart.service';
import { AuthService } from '../auth/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  cartItems: CartRequest[] = [];
  sizeCart: number = 0;
  email: string | null = '';
  constructor(
    private cookieService: CookieService,
    private cartService: CartServiceService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.cartItems = this.cartService.getCartItems();
    this.sizeCart = this.cartItems.length;
    this.email = this.authService.getEmail();
    console.log(this.email);
  }

  logout() {
    this.authService.logout();
  }
  isLoggedIn(): boolean {
    return this.authService.checkLoginStatus();
  }
}
