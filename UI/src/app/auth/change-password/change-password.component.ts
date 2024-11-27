import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css',
})
export class ChangePasswordComponent implements OnInit {
  changePassword = {
    email: '',
    password: '',
    newPassword: '',
  };

  constructor(private changePasswordService: AuthService) {}
  ngOnInit(): void {
    this.changePassword.email = this.changePasswordService.getEmail() ?? '';
  }

  onSubmit() {
    if (
      this.changePassword.email &&
      this.changePassword.password &&
      this.changePassword.newPassword
    ) {
      this.changePasswordService.changePassword(this.changePassword).subscribe(
        (response) => {
          alert('Mật khẩu đã được thay đổi thành công!');
        },
        (error) => {
          alert('Mật khẩu đã được thay đổi thành công!');
        }
      );
    }
  }
}
