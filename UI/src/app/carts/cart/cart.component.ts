import { CartRequest } from './../models/cart.request';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { CartServiceService } from '../services/cart.service';
import { elementAt } from 'rxjs';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit {
  cartItems: CartRequest[] = [];

  constructor(private cartService: CartServiceService) {}

  ngOnInit() {
    this.loadCart(); // Lấy giỏ hàng từ cookie khi component khởi tạo
  }
  sumTotalCart(): number {
    const totalQuantity = this.cartItems.reduce(
      (total: number, currentItem: CartRequest) => {
        return total + currentItem.quantity * currentItem.price;
      },
      0
    );
    return totalQuantity;
  }
  loadCart() {
    this.cartItems = this.cartService.getCartItems(); // Lấy giỏ hàng từ cookie
  }
  removeFromCart(cartRequest: CartRequest) {
    this.cartService.removeFromCart(cartRequest);
    this.loadCart();
  }

  increaseQuantity(cartItem: CartRequest): void {
    this.cartService.updateQuantity(cartItem, '+');
    this.loadCart();
  }
  decreaseQuantity(cartItem: CartRequest): void {
    this.cartService.updateQuantity(cartItem, '-');
    this.loadCart();
  }
}
