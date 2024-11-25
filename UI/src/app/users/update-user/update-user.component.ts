import { AuthService } from './../../auth/auth.service';
import { Component, OnInit } from '@angular/core';
import { UserRequestViewModel } from '../Model/user.model';
import { UserService } from '../user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-user',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './update-user.component.html',
  styleUrl: './update-user.component.css',
})
export class UpdateUserComponent implements OnInit {
  email: string | null = ''; // Khai báo email là null
  user: any = null;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService // Thêm AuthService vào constructor
  ) {}

  ngOnInit(): void {
    // Lấy email từ localStorage thông qua AuthService
    const storedEmail = this.authService.getEmail();
    if (storedEmail) {
      this.email = storedEmail;
      this.getUserDetails(); // Tự động lấy thông tin người dùng khi có email
    } else {
      this.errorMessage = 'Không tìm thấy email trong localStorage.';
    }
  }

  // Lấy thông tin người dùng từ email
  getUserDetails(): void {
    if (this.email) {
      this.userService.getUserByEmail(this.email).subscribe({
        next: (data) => {
          this.user = data;
          this.errorMessage = '';
        },
        error: (err) => {
          this.errorMessage = 'Không thể lấy thông tin người dùng.';
          this.user = null;
        },
      });
    }
  }

  // Cập nhật thông tin người dùng
  updateUser(): void {
    if (this.user && this.email) {
      this.userService.updateUser(this.email, this.user).subscribe({
        next: (response) => {
          this.successMessage = 'Cập nhật thành công!';
          this.router.navigate(['/user-list']);
        },
        error: (err) => {
          this.errorMessage = 'Cập nhật thất bại.';
        },
      });
    } else {
      this.errorMessage = 'Dữ liệu không hợp lệ.';
    }
  }
}
