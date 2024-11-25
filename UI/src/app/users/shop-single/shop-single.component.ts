import { CartServiceService } from './../../carts/services/cart.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ProductViewModel } from '../models/product.viewmodel';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CartRequest } from '../../carts/models/cart.request';

@Component({
  selector: 'app-shop-single',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './shop-single.component.html',
  styleUrl: './shop-single.component.css',
})
export class ShopSingleComponent implements OnInit {
  /**
   *
   */
  product: ProductViewModel = {
    // Khởi tạo giá trị mặc định
    id: 0,
    name: '',
    description: '',
    price: 0,
    categoryId: 0,
    categoryName: '',
    createdAt: '',
    updatedAt: '',
    sizeId: [],
    sizeName: [],
    quantity: [],
    url: '',
  };
  selectedSizeIndex?: number; // Index của Size được chọn
  maxQuantity: number = 0; // Số lượng tối đa cho Size được chọn
  inputQuantity: number = 1; // Số lượng người dùng chọn
  constructor(
    private router: Router,
    private cartService: CartServiceService
  ) {}

  ngOnInit(): void {
    // Lấy dữ liệu từ state
    this.product = history.state.product;

    if (this.product) {
      console.log('Received product:', this.product);
    } else {
      console.log('No product data found.');
    }
  }

  // add to cart
  addToCart(product: ProductViewModel, newQuantity: number): void {
    if (this.selectedSizeIndex === undefined) {
      alert('Vui lòng chọn size trước khi thêm vào giỏ hàng.');
      return;
    }

    const selectedSizeId = product.sizeId[this.selectedSizeIndex];
    const selectedSizeName = product.sizeName[this.selectedSizeIndex];

    let cartRequest: CartRequest = {
      productId: product.id,
      quantity: newQuantity,
      price: product.price,
      nameProdcut: product.name,
      url: product.url || '', // Xử lý trường hợp url null
      sizeId: selectedSizeId,
      sizeName: selectedSizeName,
    };

    this.cartService.addToCart(cartRequest);
    alert('Sản phẩm đã được thêm vào giỏ hàng!');
  }

  // Khi chọn Size, cập nhật maxQuantity và reset inputQuantity
  onSizeChange(index: number): void {
    this.selectedSizeIndex = index;
    this.maxQuantity = this.product.quantity[index] || 0; // Lấy số lượng tương ứng từ product.quantity
    this.inputQuantity = 1; // Reset input về 1
  }

  // Giảm số lượng (không dưới 1)
  decrementQuantity(): void {
    if (this.inputQuantity > 1) {
      this.inputQuantity--;
    }
  }

  // Tăng số lượng (không vượt quá maxQuantity)
  incrementQuantity(): void {
    if (this.inputQuantity < this.maxQuantity) {
      this.inputQuantity++;
    }
  }
}
