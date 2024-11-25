import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-get-by-email',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './get-by-email.component.html',
  styleUrl: './get-by-email.component.css',
})
export class GetByEmailComponent implements OnInit {
  email: string | null = '';
  user: any = null;
  errorMessage: string = '';

  constructor(
    private userService: UserService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Lấy email từ localStorage
    const storedEmail = this.authService.getEmail();
    if (storedEmail) {
      this.email = storedEmail;
      this.getUserByEmail(); // Tự động lấy thông tin người dùng
    } else {
      this.errorMessage = 'Không tìm thấy email trong localStorage.';
    }
  }

  getUserByEmail(): void {
    this.userService.getUserByEmail(this.email).subscribe({
      next: (data) => {
        this.user = data;
        this.errorMessage = '';
      },
      error: (err) => {
        this.errorMessage = err;
        this.user = null;
      },
    });
  }
}
