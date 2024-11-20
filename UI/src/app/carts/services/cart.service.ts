import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { CartRequest } from '../models/cart.request'; // Đường dẫn tùy theo vị trí của CartRequest
import e from 'express';

@Injectable({
  providedIn: 'root',
})
export class CartServiceService {
  private cartCookieName = 'cart'; // Tên cookie chứa giỏ hàng

  constructor(private cookieService: CookieService) {}

  // Lấy giỏ hàng từ cookie
  getCartItems(): CartRequest[] {
    const cart = this.cookieService.get(this.cartCookieName);
    if (cart) {
      return JSON.parse(cart); // Nếu cookie tồn tại, parse từ JSON và trả về
    } else {
      // Nếu cookie chưa tồn tại, tạo cookie với giá trị mảng rỗng
      const emptyCart: CartRequest[] = [];
      this.cookieService.set(this.cartCookieName, JSON.stringify(emptyCart), {
        expires: 7,
      });
      return emptyCart; // Trả về mảng rỗng
    }
  }

  // Lưu giỏ hàng vào cookie
  saveCartItems(cartItems: CartRequest[]): void {
    const cartJson = JSON.stringify(cartItems);
    this.cookieService.set(this.cartCookieName, cartJson, { expires: 7 }); // Lưu cookie trong 7 ngày
  }

  // Thêm sản phẩm vào giỏ hàng
  addToCart(cartRequest: CartRequest): void {
    const cartItems = this.getCartItems();

    // Kiểm tra nếu sản phẩm đã có trong giỏ hàng, cập nhật số lượng
    const existingItem = cartItems.find(
      (item) =>
        item.productId === cartRequest.productId &&
        item.sizeId === cartRequest.sizeId
    );

    if (existingItem) {
      existingItem.quantity += cartRequest.quantity; // Cộng thêm số lượng nếu sản phẩm đã có
    } else {
      // Nếu sản phẩm chưa có, thêm mới vào giỏ hàng
      cartItems.push(cartRequest);
    }

    // Lưu giỏ hàng vào cookie
    this.saveCartItems(cartItems);
  }

  // Cập nhật số lượng sản phẩm trong giỏ hàng
  updateQuantity(cartRequest: CartRequest, operator: string): void {
    const cartItems = this.getCartItems();
    const existingItem = cartItems.find(
      (item) =>
        item.productId === cartRequest.productId &&
        item.sizeId === cartRequest.sizeId
    );

    if (!existingItem) return; // Nếu sản phẩm không tồn tại trong giỏ hàng, thoát sớm

    // Cập nhật số lượng dựa trên operator
    existingItem.quantity += operator === '+' ? 1 : -1;

    // Xử lý khi số lượng <= 0
    if (existingItem.quantity <= 0) {
      this.removeFromCart(existingItem);
    } else {
      this.saveCartItems(cartItems); // Lưu lại danh sách giỏ hàng
    }
  }

  // Xóa sản phẩm khỏi giỏ hàng
  removeFromCart(cartRequest: CartRequest): void {
    let cartItems = this.getCartItems();
    cartItems = cartItems.filter(
      (item) =>
        !(
          item.productId === cartRequest.productId &&
          item.sizeId === cartRequest.sizeId
        )
    ); // Lọc bỏ sản phẩm muốn xóa
    this.saveCartItems(cartItems); // Lưu lại giỏ hàng sau khi xóa
  }

  // Xóa tất cả sản phẩm trong giỏ hàng
  clearCart(): void {
    this.cookieService.delete(this.cartCookieName); // Xóa giỏ hàng khỏi cookie
  }

  // Tính tổng số lượng sản phẩm trong giỏ hàng
  getCartSize(): number {
    return this.getCartItems().length;
  }
}
