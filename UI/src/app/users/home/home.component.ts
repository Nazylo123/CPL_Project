import { CartServiceService } from './../../carts/services/cart.service';
import { CommonModule } from '@angular/common';
import { MomoRequest } from './../../carts/models/momo.request';
import { CheckoutService } from './../../carts/services/checkout.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  showPopup = false; // Biến này để điều khiển việc hiển thị popup
  momoRequest: MomoRequest | null = null;
  popupTitle = '';
  popupMessage = '';
  constructor(
    private route: ActivatedRoute,
    private checkoutService: CheckoutService,
    private cartService: CartServiceService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.getPaymentResult();
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
          this.popupTitle = 'Xác Nhận';
          this.popupMessage = 'Thanh toán thành công';
          this.checkOutCart();
          this.showPopup = true; // Hiển thị popup
        }
      },
      (error) => {
        console.error('Error getting payment result', error);
      }
    );
  }

  checkOutCart() {
    this.checkoutService.checkout(this.cartService.getCartItems()).subscribe(
      (response) => {
        this.cartService.clearCart();
        alert('Thanh toán thành công! Cảm ơn bạn đã mua sắm.');
        this.router.navigate(['']);
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
