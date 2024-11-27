import { FormsModule } from '@angular/forms';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  // Forgot password state
  isForgotPasswordMode: boolean = false;
  forgotPasswordEmail: string = '';
  forgotPasswordMessage: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private http: HttpClient
  ) {}

  onLogin() {
    this.authService
      .login({ email: this.email, password: this.password })
      .subscribe({
        next: (response: any) => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('roles', JSON.stringify(response.roles));
          localStorage.setItem('email', response.email);
          this.router.navigate(['/dashboard']);
        },
        error: () => {
          this.errorMessage = 'Invalid email or password';
        },
      });
  }

  // Handle Forgot Password
  toggleForgotPasswordMode() {
    this.isForgotPasswordMode = !this.isForgotPasswordMode;
    this.forgotPasswordMessage = '';
    this.forgotPasswordEmail = '';
  }

  onForgotPassword() {
    if (!this.forgotPasswordEmail) {
      this.forgotPasswordMessage = 'Email không được để trống.';
      return;
    }

    const apiUrl = 'https://localhost:7102/api/Auth/forgot-password';

    this.http.post(apiUrl, { email: this.forgotPasswordEmail }).subscribe({
      next: () => {
        alert('Đã gửi email đặt lại mật khẩu. Vui lòng kiểm tra hộp thư.');
        this.forgotPasswordMessage =
          'Đã gửi email đặt lại mật khẩu. Vui lòng kiểm tra hộp thư.';
      },
      error: (err) => {
        this.forgotPasswordMessage =
          err.error || 'Đã xảy ra lỗi. Vui lòng thử lại.';
      },
    });
  }
}
