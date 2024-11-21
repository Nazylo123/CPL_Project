import { CheckoutService } from './../services/checkout.service';
import { MomoRequest } from './../models/momo.request';
import { CartRequest } from './../models/cart.request';
import { CookieService } from 'ngx-cookie-service';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Product } from '../models/product';
import {  ActivatedRoute, RouterLink } from '@angular/router';
import { Router  } from '@angular/router';
import { CartServiceService } from '../services/cart.service';
import { BrowserModule } from '@angular/platform-browser';


@Component({
  selector: 'app-testcart',
  standalone: true,
  imports: [CommonModule, RouterLink, ],
  templateUrl: './testcart.component.html',
  styleUrl: './testcart.component.css',
})
export class TestcartComponent implements OnInit {

  momoRequest: MomoRequest| null  = null;

  constructor(
    private cookieService: CookieService,
    private cartService: CartServiceService,
    private router: Router,
    private route: ActivatedRoute,
    private checkoutService: CheckoutService
  ) {

   

  }

  showPopup = false;  // Biến này để điều khiển việc hiển thị popup
  products: Product[] = [];
  
  ngOnInit(): void {
    this.products = [
      {
        id: 1,
        name: 'Sneaker XYZ',
        description: 'Comfortable and stylish sneakers',
        price: 12000,
        categoryId: 1,
        category: { id: 1, name: 'Shoes' },
        createdAt: new Date('2024-01-01'),
        updatedAt: new Date('2024-01-15'),
        productSizeStocks: [
          { sizeId: 1, size: 'M', stock: 10 },
          { sizeId: 2, size: 'L', stock: 5 },
        ],
        productImages: [
          { id: 1, url: '../../../../assets/images/cloth_1.jpg', productId: 1 },
          { id: 2, url: '../../../../assets/images/cloth_2.jpg', productId: 1 },
        ],
      },
    ];
    this.getPaymentResult();
  }

  addToCart(product: Product, newQuantity: number): void {
    let cartRequest: CartRequest = {
      productId: product.id,
      quantity: newQuantity,
      price: product.price,
      nameProdcut: product.name,
      url: product.productImages[0].url,
      sizeId: product.productSizeStocks[0].sizeId,
      sizeName: product.productSizeStocks[0].size,
    };
    this.cartService.addToCart(cartRequest);
  }


  closePopup() {
    this.showPopup = false;
  }

 getPaymentResult() {
  // Lấy query string từ URL hiện tại
  const queryString = this.route.snapshot.queryParams;

  // Biến đổi queryParams thành chuỗi query string
  const query = new URLSearchParams(queryString).toString();

  // Gọi hàm API với query string
  this.checkoutService.getResultmomo(query).subscribe(
    (result) => {
      this.momoRequest = result;
      console.log('Payment result:', this.momoRequest);

      // Kiểm tra nếu thanh toán thành công
      if (this.momoRequest.message === 'Success') {
        this.showPopup = true; // Hiển thị popup
      }
    },
    (error) => {
      console.error('Error getting payment result', error);
    }
  );
}

 
}
