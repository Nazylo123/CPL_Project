import { CartRequest } from './../models/cart.request';
import { CookieService } from 'ngx-cookie-service';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Product } from '../models/product';
import { CartServiceService } from '../services/cart.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-testcart',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './testcart.component.html',
  styleUrl: './testcart.component.css',
})
export class TestcartComponent implements OnInit {
  constructor(
    private cookieService: CookieService,
    private cartService: CartServiceService
  ) {}
  products: Product[] = [];

  ngOnInit(): void {
    this.products = [
      {
        id: 1,
        name: 'Sneaker XYZ',
        description: 'Comfortable and stylish sneakers',
        price: 120,
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
}
