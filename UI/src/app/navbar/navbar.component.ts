import { ProductsService } from './../users/services/products.service';
import { CartRequest } from './../carts/models/cart.request';
import { CookieService } from 'ngx-cookie-service';
import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CartServiceService } from '../carts/services/cart.service';

import { FormsModule } from '@angular/forms';

import { AuthService } from '../auth/auth.service';

import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,

  imports: [RouterLink, FormsModule, CommonModule],


  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  cartItems: CartRequest[] = [];
  sizeCart: number = 0;

  searchTerm: string = '';

  suggestions: string[] = []; // Danh sách gợi ý

  email: string | null = '';

  constructor(
    private cookieService: CookieService,
    private cartService: CartServiceService,
    private router: Router,
    private productService: ProductsService,
    private authService: AuthService

  ) {}

  ngOnInit(): void {
    this.cartItems = this.cartService.getCartItems();
    this.sizeCart = this.cartItems.length;
    this.email = this.authService.getEmail();
    console.log(this.email);
  }


  onSearchTermChange(): void {
    if (this.searchTerm.trim() !== '') {
      this.productService.getSearchSuggestions(this.searchTerm).subscribe({
        next: (response) => (this.suggestions = response),
        error: (err) => console.error('Error fetching suggestions', err),
      });
    } else {
      this.suggestions = [];
    }
  }

  // Khi người dùng chọn một gợi ý
  selectSuggestion(suggestion: string): void {
    this.searchTerm = suggestion; // Gán từ khóa được chọn
    this.suggestions = []; // Ẩn danh sách gợi ý
    this.onSearch(); // Thực hiện tìm kiếm
  }

  onSearch(): void {
    this.suggestions = [];
    this.router.navigate(['/shop'], {
      queryParams: { search: this.searchTerm },
    });
  }

  logout() {
    this.authService.logout();
  }

  isLoggedIn(): boolean {
    return this.authService.checkLoginStatus();

  }


}
