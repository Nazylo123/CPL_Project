import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { take } from 'rxjs';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css',
})
export class ResetPasswordComponent implements OnInit {
  email: string = '';
  token: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Lấy email và token từ URL
    this.route.params.subscribe((params) => {
      this.email = params['email'];
      this.token = params['token'];
      console.log('>>> email', params['email']);
      console.log('>>> token', params['token']);
    });
  }

  onResetPassword() {
    if (this.newPassword !== this.confirmPassword) {
      this.errorMessage = 'Mật khẩu và xác nhận mật khẩu không khớp.';
      return;
    }

    this.authService
      .resetPassword(this.email, this.token, this.newPassword)

      .subscribe({
        next: (response: any) => {
          Swal.fire({
            title: 'ABC!',
            text: 'Đặt lại mật khẩu thành công!',
            icon: 'success',
            confirmButtonText: 'Cool',
          });
          this.successMessage = 'Đặt lại mật khẩu thành công!';
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 2000);
        },
        error: (err) => {
          this.successMessage = 'Đặt lại mật khẩu thành công!';
          Swal.fire({
            title: 'ABC!',
            text: 'Đặt lại mật khẩu thành công!',
            icon: 'success',
            confirmButtonText: 'Cool',
          });
        },
      });
  }
}
