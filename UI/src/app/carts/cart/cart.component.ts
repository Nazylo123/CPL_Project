import { CheckoutService } from './../services/checkout.service';
import { MomoCreatePaymentResponse } from './../models/momo-create-payment-response';
import { MomoRequest } from './../models/momo.request';
import { CartRequest } from './../models/cart.request';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { elementAt, Observable } from 'rxjs';
import { CartServiceService } from '../services/cart.service';

import { response } from 'express';
import { error } from 'console';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule,FormsModule ],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit {
  cartItems: CartRequest[] = [];
  model: MomoRequest;
  momoCreatePaymentResponse: MomoCreatePaymentResponse | null = null; // Khởi tạo là null
  constructor(
    private cartService: CartServiceService,
    private checkoutService: CheckoutService,
    private router: Router
  ) {
    // Khởi tạo model với các giá trị mặc định
   this.model = {
      orderId: 'string', // Bạn có thể gán giá trị mặc định hoặc lấy từ dữ liệu khác
      amount: '10000', // Tương tự cho amount
      fullName: 'dat', // Tên đầy đủ
      orderInfo: 'chuyen tien', // Thông tin đơn hàng
      message: 'string', // Tin nhắn thanh toán
      };
  }

  ngOnInit() {
    this.loadCart(); // Lấy giỏ hàng từ cookie khi component khởi tạo
  }
  sumTotalCart(): string {
    const totalQuantity = this.cartItems.reduce(
      (total: number, currentItem: CartRequest) => {
        return total + currentItem.quantity * currentItem.price;
      },
      0
    );
    this.model.amount = totalQuantity.toString();
    return this.model.amount;
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

  createPayment() {
    this.checkoutService.createMomoPayment(this.model).subscribe(
      (response: MomoCreatePaymentResponse) => {
        this.momoCreatePaymentResponse = response;
        console.log('Payment created successfully', response);
        window.location.href = response.payUrl;
      },
      (error) => {
        console.error('Error creating payment', error);
      }
    );

   
  }

 
}
